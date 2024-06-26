using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;
using SistemaHoteis.Repositories.Interfaces;

namespace SistemaHoteis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HospedeController : ControllerBase
{
    private readonly IHospedeRepositorio _hospedeRepositorio;

    public HospedeController(IHospedeRepositorio hospedeRepositorio)
    {
        _hospedeRepositorio = hospedeRepositorio;
    }

    /// <summary>
    /// Cadastrar um hospede
    /// </summary>
    /// <remarks>
    /// {"name":"string","cpf":"string", "dataNascimento":"DateTime", "telefone:"string"}
    /// </remarks>
    /// <param name="hospedeDto">Dados do hospede</param>
    /// <returns>Objeto hospede recém-criado</returns>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Hospede>> AdicionarHospede([FromBody] HospedeDto hospedeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Hospede hospede = await _hospedeRepositorio.AdicionarHospede(hospedeDto);
        return CreatedAtAction(nameof(BuscarHospedePorId), new { id = hospede.Id }, hospede);
    }

    /// <summary>
    /// Obter todos os hospedes
    /// </summary>
    /// <remarks>
    /// {"name":"string","cpf":"string", "dataNascimento":"DateTime", "telefone:"string","checkins":"ICollection", "hoteis":"ICollection"}
    /// </remarks>
    /// <returns>Coleção de hospedes</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Hospede>>> BuscarHospedes()
    {
        List<Hospede> listHospedes = await _hospedeRepositorio.BuscarHospedes();
        return Ok(listHospedes);
    }

    /// <summary>
    /// Retorna um hospede baseado no ID passado
    /// </summary>
    /// <param name="id">Identificador do hospede</param>
    /// <returns>Dados de um hospede específico</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not Found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hospede>> BuscarHospedePorId(int id)
    {
        Hospede hospedeId = await _hospedeRepositorio.BuscarHospedePorId(id);

        if (hospedeId == null)
        {
            return NotFound();
        }

        return Ok(hospedeId);
    }

    /// <summary>
    /// Atualizar um hospede
    /// </summary>
    /// <remarks>
    /// {"name":"string","cpf":"string", "dataNascimento":"DateTime", "telefone:"string"}
    /// </remarks>
    /// <param name="id">Identificador do hospede</param>
    /// <param name="hospedeDto">Dados do hotel</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Hospede>> AtualizarHospede([FromBody] HospedeDto hospedeDto, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (hospedeDto.Id != id)
        {
            return NotFound();
        }

        await _hospedeRepositorio.AtualizarHospede(hospedeDto, id);
        return NoContent();
    }

    /// <summary>
    /// Atualizar parcialmente um hospede
    /// </summary>
    /// <remarks>
    /// {"name":"string","cpf":"string", "dataNascimento":"DateTime", "telefone:"string"}
    /// </remarks>
    /// <param name="id">Identificador do hospede</param>
    /// <param name="document">Dados do documento</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AtualizarParcialmenteHospede(int id, [FromBody] JsonPatchDocument<HospedeDto> document)
    {
        if (document == null)
        {
            return BadRequest("Documento parcial não pode ser nulo");
        }

        Hospede hospede = await _hospedeRepositorio.BuscarHospedePorId(id);
        
        if (hospede == null)
        {
            return NotFound();
        }

        HospedeDto hospedeDto = new HospedeDto
        {
            Name = hospede.Name,
            CPF = hospede.CPF,
            DataNascimento = hospede.DataNascimento,
            Telefone = hospede.Telefone,
        };

        document.ApplyTo(hospedeDto, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        await _hospedeRepositorio.AtualizarParcialmenteHospede(id, document);
        return NoContent();
    }

    /// <summary>
    /// Excluir um hospede
    /// </summary>
    /// <param name="id">Identificador de hospede</param>
    /// <returns>Conteúdo vazio</returns>
    /// <response code="204">No Content</response>
    /// <response code="404">Not Found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hospede>> RemoverHospede(int id)
    {
        bool hospedeRemovido = await _hospedeRepositorio.RemoverHospede(id);

        if (!hospedeRemovido)
        {
            return NotFound();
        }

        return NoContent();
    }
}

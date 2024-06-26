using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;
using SistemaHoteis.Repositories;
using SistemaHoteis.Repositories.Interfaces;

namespace SistemaHoteis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelRepositorio _hotelRepositorio;

    public HotelController(IHotelRepositorio hotelRepositorio)
    {
        _hotelRepositorio = hotelRepositorio;
    }

    /// <summary>
    /// Cadastrar um hotel
    /// </summary>
    /// <remarks>
    /// {"name":"string","description":"string", "endereco":"string", "numeroDeQuartos:"int"}
    /// </remarks>
    /// <param name="hotelDto">Dados do hotel</param>
    /// <returns>Objeto hotel recém-criado</returns>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Hotel>> AdicionarHotel([FromBody] HotelDto hotelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Hotel hotel = await _hotelRepositorio.AdicionarHotel(hotelDto);   
        return CreatedAtAction(nameof(BuscarHotelPorId), new { id = hotel.Id }, hotel);
    }

    /// <summary>
    /// Obter todos os hotéis
    /// </summary>
    /// <remarks>
    /// {"name":"string","description":"string", "endereco":"string", "numeroDeQuartos":"int", "checkins":"ICollection"}
    /// </remarks>
    /// <returns>Coleção de hotéis</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<HotelDto>>> BuscarHoteis()
    {
        List<HotelDto> listaHoteis = await _hotelRepositorio.BuscarHoteis();
        return Ok(listaHoteis);
    }

    /// <summary>
    /// Retorna um hotel baseado no ID passado
    /// </summary>
    /// <param name="id">Identificador do hotel</param>
    /// <returns>Dados de um hotel específico</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not Found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hotel>> BuscarHotelPorId(Guid id)
    {
        Hotel hotelId = await _hotelRepositorio.BuscarHotelPorIdEntidade(id);

        if (hotelId == null)
        {
            return NotFound();
        }

        return Ok(hotelId);
    }


    /// <summary>
    /// Atualizar um hotel
    /// </summary>
    /// <remarks>
    /// {"name":"string","description":"string", "endereco":"string", "numeroDeQuartos:"int"}
    /// </remarks>
    /// <param name="id">Identificador do hotel</param>
    /// <param name="hotelDto">Dados do hotel</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Hotel>> AtualizarHotel([FromBody] HotelDto hotelDto, Guid id)
    {
        Hotel hotel = await _hotelRepositorio.BuscarHotelPorIdEntidade(id);

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (hotel.Id != id)
        {
            return NotFound();
        }
        
        await _hotelRepositorio.AtualizarHotel(hotelDto, id);
        return NoContent();
    }

    /// <summary>
    /// Atualizar parcialmente um hotel
    /// </summary>
    /// <remarks>
    /// {"name":"string","description":"string", "endereco":"string", "numeroDeQuartos:"int"}
    /// </remarks>
    /// <param name="id">Identificador do hotel</param>
    /// <param name="document">Dados do documento</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Hotel>> AtualizarParcialmenteHotel(Guid id,[FromBody] JsonPatchDocument<HotelDto> document)
    {
        if (document == null)
        {
            return BadRequest("Documento parcial não pode ser nulo");
        }

        Hotel hotel = await _hotelRepositorio.BuscarHotelPorIdEntidade(id);
        
        if (hotel == null)
        {
            return NotFound();
        }
        HotelDto hotelDto = new HotelDto
        {
            Name = hotel.Name,
            Description = hotel.Description,
            Endereco = hotel.Endereco,
            NumeroDeQuartos = hotel.NumeroDeQuartos
        };

        document.ApplyTo(hotelDto, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        await _hotelRepositorio.AtualizarParcialmenteHotel(id, document);
        return NoContent();
    }

    /// <summary>
    /// Excluir um hotel
    /// </summary>
    /// <param name="id">Identificador de hotel</param>
    /// <returns>Conteúdo vazio</returns>
    /// <response code="204">No Content</response>
    /// <response code="404">Not Found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hotel>> DeletarHotel(Guid id)
    {
        bool apagado = await _hotelRepositorio.DeletarHotel(id);

        if (!apagado)
        {
            return BadRequest();
        }

        return NoContent();
    }

}

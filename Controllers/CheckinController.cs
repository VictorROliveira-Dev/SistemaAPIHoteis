using Microsoft.AspNetCore.Mvc;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;
using SistemaHoteis.Repositories.Interfaces;

namespace SistemaHoteis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckinController : ControllerBase
{
    private readonly ICheckinRepositorio _checkinRepositorio;

    public CheckinController(ICheckinRepositorio checkinRepositorio)
    {
        _checkinRepositorio = checkinRepositorio;
    }

    /// <summary>
    /// Criar um check-in
    /// </summary>
    /// <remarks>
    /// {"dataCheckin":"DateTime","dataCheckout":"DateTime", "hotelId":"Guid", "hospedeId:"int"}
    /// </remarks>
    /// <param name="checkinDto">Dados do check-in</param>
    /// <returns>Objeto check-in recém-criado</returns>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CheckIn>> AdicionarCheckin([FromBody] CheckinDto checkinDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        CheckIn checkin = await _checkinRepositorio.AdicionarCheckin(checkinDto);
        return CreatedAtAction(nameof(BuscarChekcinPorId), new { id = checkin.Id }, checkin);
    }

    /// <summary>
    /// Obter todos os check-ins
    /// </summary>
    /// <remarks>
    /// {"dataCheckin":"DateTime","dataCheckout":"DateTime", "hotel":"HotelId", "hospede:"HospedeId"}
    /// </remarks>
    /// <returns>Coleção de check-in</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CheckIn>>> BuscarCheckins()
    {
        List<CheckIn> checkins = await _checkinRepositorio.BuscarCheckins();
        return Ok(checkins);
    }

    /// <summary>
    /// Retorna um check-in baseado no ID passado
    /// </summary>
    /// <param name="id">Identificador do check-in</param>
    /// <returns>Dados de um check-in específico</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not Found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckIn>> BuscarChekcinPorId(Guid id)
    {
        CheckIn checkin = await _checkinRepositorio.BuscarChekcinPorId(id);

        if (checkin == null)
        {
            return NotFound();
        }

        return Ok(checkin);
    }

    /// <summary>
    /// Atualizar um check-in
    /// </summary>
    /// <remarks>
    /// {"dataCheckin":"DateTime","dataCheckout":"DateTime", "hotelId":"Guid", "hospedeId:"int"}
    /// </remarks>
    /// <param name="id">Identificador do check-in</param>
    /// <param name="checkinDto">Dados do check-in</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CheckIn>> AtualizarCheckin([FromBody] CheckinDto checkinDto, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        await _checkinRepositorio.AtualizarCheckin(checkinDto, id);
        
        return NoContent();
    }

    /// <summary>
    /// Excluir um check-in
    /// </summary>
    /// <param name="id">Identificador de check-in</param>
    /// <returns>Conteúdo vazio</returns>
    /// <response code="204">No Content</response>
    /// <response code="404">Not Found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckIn>> RemoverCheckin(Guid id)
    {
        bool checkinApagado = await _checkinRepositorio.RemoverCheckin(id);

        if (!checkinApagado)
        {
            return BadRequest();
        }

        return NoContent();
    }
}

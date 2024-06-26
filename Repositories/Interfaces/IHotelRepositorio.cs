using Microsoft.AspNetCore.JsonPatch;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;

namespace SistemaHoteis.Repositories.Interfaces;

public interface IHotelRepositorio
{
    Task<List<HotelDto>> BuscarHoteis();
    Task<HotelDto> BuscarHotelPorId(Guid id);
    Task<Hotel> BuscarHotelPorIdEntidade(Guid id);
    Task<Hotel> AdicionarHotel(HotelDto hotelDto);
    Task<Hotel> AtualizarHotel(HotelDto hotelDto, Guid id);
    Task<bool> DeletarHotel(Guid id);
    Task AtualizarParcialmenteHotel(Guid id, JsonPatchDocument<HotelDto> document);
}

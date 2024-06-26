using Microsoft.AspNetCore.JsonPatch;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;

namespace SistemaHoteis.Repositories.Interfaces;

public interface ICheckinRepositorio
{
    Task<CheckIn> AdicionarCheckin(CheckinDto checkinDto);
    Task<CheckIn> BuscarChekcinPorId(Guid id);
    Task<List<CheckIn>> BuscarCheckins();
    Task<CheckIn> AtualizarCheckin(CheckinDto checkinDto, Guid id);
    Task<bool> RemoverCheckin(Guid id);
}

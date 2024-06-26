using Microsoft.AspNetCore.JsonPatch;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;

namespace SistemaHoteis.Repositories.Interfaces;

public interface IHospedeRepositorio
{
    Task<Hospede> AdicionarHospede(HospedeDto hospedeDto);  
    Task<List<Hospede>> BuscarHospedes();
    Task<Hospede> AtualizarHospede(HospedeDto hospedeDto, int id);
    Task<Hospede> BuscarHospedePorId(int id);   
    Task<bool> RemoverHospede(int id);
    Task AtualizarParcialmenteHospede(int id, JsonPatchDocument<HospedeDto> document);
}

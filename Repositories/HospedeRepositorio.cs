using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SistemaHoteis.Data;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;
using SistemaHoteis.Repositories.Interfaces;

namespace SistemaHoteis.Repositories;

public class HospedeRepositorio : IHospedeRepositorio
{
    private readonly AppDbContext _appDbContext;

    public HospedeRepositorio(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Hospede> AdicionarHospede(HospedeDto hospedeDto)
    {
        Hospede hospede = new Hospede
        {
            Id = hospedeDto.Id,
            Name = hospedeDto.Name,
            CPF = hospedeDto.CPF,
            DataNascimento = hospedeDto.DataNascimento,
            Telefone = hospedeDto.Telefone,
        };

        await _appDbContext.Hospedes.AddAsync(hospede);
        await _appDbContext.SaveChangesAsync();

        return hospede;
    }

    public async Task<Hospede> AtualizarHospede(HospedeDto hospedeDto, int id)
    {
        Hospede hospedeEntidade = await BuscarHospedePorId(id);

        if (hospedeEntidade == null)
        {
            throw new ArgumentException($"Hospede com id: {id} não encontrado.");
        }

        hospedeEntidade.Name = hospedeDto.Name;
        hospedeEntidade.CPF = hospedeDto.CPF;
        hospedeEntidade.DataNascimento = hospedeDto.DataNascimento;
        hospedeEntidade.Telefone = hospedeDto.Telefone;

        _appDbContext.Hospedes.Update(hospedeEntidade);
        await _appDbContext.SaveChangesAsync();

        return hospedeEntidade;

    }

    public async Task AtualizarParcialmenteHospede(int id, JsonPatchDocument<HospedeDto> document)
    {
        Hospede hospede = await _appDbContext.Hospedes.FirstOrDefaultAsync(hos => hos.Id == id);

        if (hospede == null)
        {
            throw new InvalidOperationException($"Hote com id: {id} não encontrado.");
        }

        HospedeDto hospedeDto = new HospedeDto
        {               
            Name = hospede.Name,
            CPF = hospede.CPF,
            DataNascimento = hospede.DataNascimento,
            Telefone = hospede.Telefone
        };

        document.ApplyTo(hospedeDto);

        hospede.Name = hospedeDto.Name;
        hospede.CPF = hospedeDto.CPF;
        hospede.DataNascimento = hospedeDto.DataNascimento;
        hospede.Telefone = hospedeDto.Telefone;

        _appDbContext.Hospedes.Update(hospede);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Hospede> BuscarHospedePorId(int id)
    {
        return await _appDbContext.Hospedes.Include(c => c.Checkins).ThenInclude(h => h.Hotel).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<List<Hospede>> BuscarHospedes()
    {
        return await _appDbContext.Hospedes.Include(c => c.Checkins).ThenInclude(h => h.Hotel).ToListAsync();
    }

    public async Task<bool> RemoverHospede(int id)
    {
        Hospede hospedeEntidade = await BuscarHospedePorId(id);

        if (hospedeEntidade == null)
        {
            throw new ArgumentException($"Hospede com id: {id} não encontrado.");
        }

        _appDbContext.Hospedes.Remove(hospedeEntidade);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}

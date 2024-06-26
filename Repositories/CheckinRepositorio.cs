using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SistemaHoteis.Data;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;
using SistemaHoteis.Repositories.Interfaces;

namespace SistemaHoteis.Repositories;

public class CheckinRepositorio : ICheckinRepositorio
{
    private readonly AppDbContext _appDbContext;

    public CheckinRepositorio(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<CheckIn> AdicionarCheckin(CheckinDto checkinDto)
    {
        var hotelEntidade = await _appDbContext.Hoteis.SingleOrDefaultAsync(h => h.Id == checkinDto.HotelId);

        if (hotelEntidade == null || hotelEntidade.NumeroDeQuartos <= 0)
        {
            throw new InvalidOperationException("Não há quartos disponíveis ou o hotel não foi encontrado.");
        }

        var hospede = await _appDbContext.Hospedes.SingleOrDefaultAsync(hos => hos.Id == checkinDto.HospedeId);

        if (hospede == null)
        {
            throw new InvalidOperationException("Hospede não encontrado.");
        }

        bool checkinExistente = await _appDbContext.Checkins
            .AnyAsync(c => c.HospedeId == checkinDto.HospedeId && c.HotelId == checkinDto.HotelId);

        if (checkinExistente)
        {
            throw new InvalidOperationException("O hóspede já possui um check-in ativo neste hotel.");
        }

        CheckIn checkin = new CheckIn
        {
            DataCheckin = checkinDto.DataCheckin,
            DataCheckout = checkinDto.DataCheckout,
            HotelId = checkinDto.HotelId,
            HospedeId = checkinDto.HospedeId,
        };

        await _appDbContext.Checkins.AddAsync(checkin);
        hotelEntidade.NumeroDeQuartos--;
        await _appDbContext.SaveChangesAsync();

        return checkin;
    }

    public async Task<CheckIn> AtualizarCheckin(CheckinDto checkinDto, Guid id)
    {

        CheckIn checkin = await BuscarChekcinPorId(id);

        if (checkin == null)
        {
            throw new ArgumentNullException($"Checkin com id: {id} não encontrado.");
        }

        checkin.DataCheckin = checkinDto.DataCheckin;
        checkin.DataCheckout = checkinDto.DataCheckout;

        _appDbContext.Checkins.Update(checkin);
        await _appDbContext.SaveChangesAsync();

        return checkin;
    }

    public async Task<List<CheckIn>> BuscarCheckins()
    {
        return await _appDbContext.Checkins.Include(h => h.Hotel).Include(hos => hos.Hospede).ToListAsync();
    }

    public async Task<CheckIn> BuscarChekcinPorId(Guid id)
    {
        return await _appDbContext.Checkins.Include(h => h.Hotel).Include(hos => hos.Hospede).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> RemoverCheckin(Guid id)
    {
        CheckIn checkin = await _appDbContext.Checkins.Include(c => c.Hotel).SingleOrDefaultAsync(c => c.Id == id);

        if (checkin == null)
        {
            throw new ArgumentNullException($"Checkin com id: {id} não encontrado.");
        }

        _appDbContext.Checkins.Remove(checkin);
        checkin.Hotel.NumeroDeQuartos++;
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SistemaHoteis.Data;
using SistemaHoteis.Data.Dtos;
using SistemaHoteis.Models;
using SistemaHoteis.Repositories.Interfaces;

namespace SistemaHoteis.Repositories;

public class HotelRepositorio : IHotelRepositorio
{
    private readonly AppDbContext _appDbContext;

    public HotelRepositorio(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Hotel> AdicionarHotel(HotelDto hotelDto)
    {
        Hotel hotel = new Hotel
        {
            Name = hotelDto.Name,
            Description = hotelDto.Description,
            Endereco = hotelDto.Endereco,
            NumeroDeQuartos = hotelDto.NumeroDeQuartos,
        };

        await _appDbContext.Hoteis.AddAsync(hotel);
        await _appDbContext.SaveChangesAsync();

        return hotel;
    }

    public async Task<Hotel> AtualizarHotel(HotelDto hotelDto, Guid id)
    {
        Hotel hotelEntidade = await BuscarHotelPorIdEntidade(id);

        if (hotelEntidade == null)
        {
            throw new ArgumentNullException($"Hotel com id: {id} não encontrado.");
        }

        hotelEntidade.Name = hotelDto.Name;
        hotelEntidade.Description = hotelDto.Description;
        hotelEntidade.Endereco = hotelDto.Endereco;
        hotelEntidade.NumeroDeQuartos = hotelDto.NumeroDeQuartos;

        _appDbContext.Hoteis.Update(hotelEntidade);
        await _appDbContext.SaveChangesAsync();

        return hotelEntidade;
    }

    public async Task AtualizarParcialmenteHotel(Guid id, JsonPatchDocument<HotelDto> document)
    {
        Hotel hotel = await _appDbContext.Hoteis.FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            throw new InvalidOperationException($"Hotel com id: {id} não encontrado.");
        }

        HotelDto hotelDto = new HotelDto
        {
            Name = hotel.Name,
            Description = hotel.Description,
            Endereco = hotel.Endereco,
            NumeroDeQuartos = hotel.NumeroDeQuartos
        };

        document.ApplyTo(hotelDto);

        hotel.Name = hotelDto.Name;
        hotel.Description = hotelDto.Description;
        hotel.Endereco = hotelDto.Endereco;
        hotel.NumeroDeQuartos = hotelDto.NumeroDeQuartos;

        _appDbContext.Hoteis.Update(hotel);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<HotelDto>> BuscarHoteis()
    {
        return await _appDbContext.Hoteis.Include(h => h.Checkins).ThenInclude(c => c.Hospede)
                                         .Select(h => new HotelDto
                                         {
                                             Id = h.Id,
                                             Name = h.Name,
                                             Description = h.Description,
                                             Endereco = h.Endereco,
                                             NumeroDeQuartos = h.NumeroDeQuartos,
                                             Checkins = h.Checkins.Select(c => new CheckinHospedeDto
                                             {
                                                 DataCheckin = c.DataCheckin,
                                                 DataCheckout = c.DataCheckout,
                                                 Hospede = new HospedeDto
                                                 {
                                                     Id = c.Hospede.Id,
                                                     Name = c.Hospede.Name,
                                                     CPF = c.Hospede.CPF,
                                                     DataNascimento = c.Hospede.DataNascimento,
                                                     Telefone = c.Hospede.Telefone
                                                 }
                                             }).ToList()
                                         })
                                         .ToListAsync();
    }

    public async Task<HotelDto> BuscarHotelPorId(Guid id)
    {
        return await _appDbContext.Hoteis
            .Include(h => h.Checkins)
            .ThenInclude(c => c.Hospede)
            .Where(h => h.Id == id)
            .Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Endereco = h.Endereco,
                NumeroDeQuartos = h.NumeroDeQuartos,
                Checkins = h.Checkins.Select(c => new CheckinHospedeDto
                {
                    DataCheckin = c.DataCheckin,
                    DataCheckout = c.DataCheckout,
                    Hospede = new HospedeDto
                    {
                        Id = c.Hospede.Id,
                        Name = c.Hospede.Name,
                        CPF = c.Hospede.CPF,
                        DataNascimento = c.Hospede.DataNascimento,
                        Telefone = c.Hospede.Telefone
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Hotel> BuscarHotelPorIdEntidade(Guid id)
    {
        return await _appDbContext.Hoteis.Include(h => h.Checkins).ThenInclude(c => c.Hospede).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<bool> DeletarHotel(Guid id)
    {
        Hotel hotelEntidade = await BuscarHotelPorIdEntidade(id);

        if (hotelEntidade == null)
        {
            throw new ArgumentNullException($"Hotel de id: {id} não encontrado.");
        }

        _appDbContext.Hoteis.Remove(hotelEntidade);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}

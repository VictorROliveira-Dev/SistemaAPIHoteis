using SistemaHoteis.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaHoteis.Data.Dtos;

public class CheckinDto
{
    public Guid Id { get; set; }
    public DateTime DataCheckin { get; set; }
    public DateTime? DataCheckout { get; set; }
    public Guid HotelId { get; set; }
    public int HospedeId { get; set; }
}

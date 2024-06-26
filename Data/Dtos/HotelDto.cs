using SistemaHoteis.Models;
using System.ComponentModel.DataAnnotations;

namespace SistemaHoteis.Data.Dtos;

public class HotelDto
{
    public Guid Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public int NumeroDeQuartos { get; set; }
    public List<CheckinHospedeDto> Checkins { get; set; } = new();
}

public class CheckinHospedeDto
{
    public DateTime DataCheckin { get; set; }
    public DateTime? DataCheckout { get; set; }
    public HospedeDto Hospede { get; set; } = new();
}

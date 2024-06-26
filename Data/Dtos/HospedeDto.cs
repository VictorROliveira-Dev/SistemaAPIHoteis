using System.ComponentModel.DataAnnotations;

namespace SistemaHoteis.Data.Dtos;

public class HospedeDto
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    [StringLength(15)]
    public string Telefone { get; set; } = string.Empty;
}

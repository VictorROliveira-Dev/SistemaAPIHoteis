using System.ComponentModel.DataAnnotations;

namespace SistemaHoteis.Models;

public class Hotel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    [Required]
    public int NumeroDeQuartos { get; set; } 

    public ICollection<CheckIn> Checkins { get; set; } = [];
}

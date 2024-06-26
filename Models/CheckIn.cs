using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHoteis.Models;

public class CheckIn
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime DataCheckin { get; set; } = DateTime.UtcNow;

    public DateTime? DataCheckout { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid HotelId { get; set; }

    [ForeignKey("HotelId")]
    public virtual Hotel Hotel { get; set; } 

    [Required]
    public int HospedeId {  get; set; }

    [ForeignKey("HospedeId")]
    public virtual Hospede Hospede { get; set; } 
}

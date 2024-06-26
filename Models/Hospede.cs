using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SistemaHoteis.Models;

public class Hospede
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string CPF { get; set; } = string.Empty;

    /* [Required]
    [MaxLength(11)]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF deve estar no formato ###.###.###-##")]
    public string CPF
    {
        get => CPF;
        set => CPF = FormatCpf(value);
    } */

    [Required]
    public DateTime DataNascimento { get; set; }

    [MaxLength(15)]
    public string Telefone { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<CheckIn> Checkins { get; set; } = [];


    /*private string FormatCpf(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("CPF não pode ser vazio.");
        }
        // Remove qualquer caractere não numérico.
        var onlyDigits = Regex.Replace(value, @"\D", "");

        if (onlyDigits.Length != 11)
        {
            throw new ArgumentException("CPF deve conter 11 dígitos.");
        }
        // Retorna o formato desejado do CPF.
        return Regex.Replace(onlyDigits, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
    }*/
}

using System.ComponentModel.DataAnnotations;

namespace Kcal.DTOs;

public class UpdateUserDTO
{
    public string? Name { get; set; }
    [DataType(DataType.Date)]
    public DateTime? DataNascimento { get; set; }
    [RegularExpression("^[FfMm]$", ErrorMessage = "Sexo deve ser 'F', 'f', 'M', 'm'")]
    public string? Sexo { get; set; }
    public int? Peso { get; set; }
    public int? Altura { get; set; }
}
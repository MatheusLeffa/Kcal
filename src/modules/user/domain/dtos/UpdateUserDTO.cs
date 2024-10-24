using System.ComponentModel.DataAnnotations;

namespace Kcal.src.modules.user.domain.dtos;

public class UpdateUserDTO
{
    public string? Name { get; set; }
    [DataType(DataType.Date)]
    public DateTime? DataNascimento { get; set; }
    [RegularExpression("^[FfMm]$", ErrorMessage = "Sexo deve ser 'F', 'f', 'M', 'm'")]
    public string? Sexo { get; set; }
    public int? Peso { get; set; }
    public int? Altura { get; set; }
    public string? Email { get; set; }
}
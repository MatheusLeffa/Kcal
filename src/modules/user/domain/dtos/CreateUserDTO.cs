using System.ComponentModel.DataAnnotations;
using Kcal.src.modules.user.domain.models;

namespace Kcal.src.modules.user.domain.dtos;

public class CreateUserDTO
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }
    [Required]
    [RegularExpression("^[FfMm]$", ErrorMessage = "Sexo deve ser 'F', 'f', 'M', 'm'")]
    public string Sexo { get; set; } = null!;
    [Required]
    public int Peso { get; set; }
    [Required]
    public int Altura { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public static User DtoToModel(CreateUserDTO createUserDTO)
    {
        return new User
        {
            Name = createUserDTO.Name,
            DataNascimento = createUserDTO.DataNascimento,
            Sexo = createUserDTO.Sexo,
            Peso = createUserDTO.Peso,
            Altura = createUserDTO.Altura
        };
    }
}
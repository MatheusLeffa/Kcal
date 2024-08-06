using System.ComponentModel.DataAnnotations;
using Kcal.Models;

namespace Kcal.DTOs;

public class UserDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }
    [MaxLength(1)]
    [RegularExpression("^[FfMm]$", ErrorMessage = "Sexo deve ser 'F', 'f', 'M', 'm'")]
    public string Sexo { get; set; } = null!;
    public int Peso { get; set; }
    public int Altura { get; set; }
    public int MetabolismoBasal { get; set; }
    [EmailAddress]
    public string Email { get; set; } = null!;
    public IEnumerable<ConsumedProductsDTO> ConsumedProducts { get; set; } = null!;



    public static UserDTO? ModelToDto(User? user)
    {
        if (user == null) return null;

        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            DataNascimento = user.DataNascimento,
            Sexo = user.Sexo,
            Peso = user.Peso,
            Altura = user.Altura,
            MetabolismoBasal = user.MetabolismoBasal,
            Email = user.Email,
            ConsumedProducts = user.ConsumedProducts.Select(ConsumedProductsDTO.ModelToDto)
        };
    }
}
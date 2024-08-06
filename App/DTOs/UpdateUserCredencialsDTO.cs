using System.ComponentModel.DataAnnotations;

namespace Kcal.App.DTOs;

public class UpdateUserCredencialsDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;
}
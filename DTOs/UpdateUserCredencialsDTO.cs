using System.ComponentModel.DataAnnotations;

namespace Kcal.DTOs;

public class UpdateUserCredencialsDTO
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;
}
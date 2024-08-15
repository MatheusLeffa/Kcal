using System.ComponentModel.DataAnnotations;

namespace Kcal.App.DTOs;

public class UpdateUserCredencialsDTO
{
    [Required]
    [EmailAddress]
    public string EmailNovo { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string SenhaNova { get; set; } = null!;
}
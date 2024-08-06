using System.ComponentModel.DataAnnotations;

namespace Kcal.App.Models;

public class ConsumedProducts
{
    public Product Product { get; set; } = null!;
    public Guid ProductId { get; set; }
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public int Quantidade { get; set; }
    [DataType(DataType.Date)]
    public DateTime DataConsumo { get; set; }
}
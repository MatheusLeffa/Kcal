using Kcal.src.modules.product.domain.models;
using Kcal.src.modules.user.domain.models;
using System.ComponentModel.DataAnnotations;

namespace Kcal.src.modules.consumedProducts.domain.models;

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
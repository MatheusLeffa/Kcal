using Kcal.src.modules.consumedProducts.domain.models;
using Kcal.src.modules.product.domain.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kcal.src.modules.user.domain.models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }
    [MaxLength(2)]
    public string Sexo { get; set; } = null!;
    public int Peso { get; set; }
    public int Altura { get; set; }
    public int MetabolismoBasal { get; set; }
    [EmailAddress]
    public string Email { get; set; } = null!;
    public DateTime DataCadastro { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public List<ConsumedProducts> ConsumedProducts { get; set; } = new List<ConsumedProducts>();
}
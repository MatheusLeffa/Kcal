using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kcal.Models;

public class Product
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid ProductId { get; set; }

    [MaxLength(50, ErrorMessage = "Nome não pode ter mais de 50 characteres!")]
    public string Nome { get; set; } = null!;
    public string? Marca { get; set; }
    public int? Gramas { get; set; }
    public int Kcal { get; set; }
    public string Categoria { get; set; } = null!;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DataCadastro { get; set; }
    public List<User> Users { get; set; } = new List<User>();
    public List<ConsumedProducts> ConsumedProducts { get; set; } = new List<ConsumedProducts>();
}
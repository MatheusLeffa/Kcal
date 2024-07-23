using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kcal.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DataNascimento { get; set; }
    [MaxLength(2)]
    public string Sexo { get; set; } = null!;
    public int Peso { get; set; }
    public int Altura { get; set; }
    public int MetabolismoBasal { get; set; }
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    [MaxLength(50)]
    public string Senha { get; set; } = null!;
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DataCadastro { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public List<ConsumedProducts> ConsumedProducts { get; set; } = new List<ConsumedProducts>();
}
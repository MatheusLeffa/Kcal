namespace Kcal.DTOs;

class CreateProduct
{
    public string Nome { get; set; } = null!;
    public string? Marca { get; set; }
    public int? Gramas { get; set; }
    public int Kcal { get; set; }
    public string Categoria { get; set; } = null!;
}
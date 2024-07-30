using Kcal.Models;

namespace Kcal.DTOs;

class ProductDTO
{
    public Guid ProductId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Marca { get; set; }
    public int? Gramas { get; set; }
    public int Kcal { get; set; }
    public string Categoria { get; set; } = null!;

    public static ProductDTO? ModelToDto(Product? product)
    {
        if (product == null) return null;

        return new ProductDTO
        {
            ProductId = product.ProductId,
            Nome = product.Nome,
            Marca = product.Marca,
            Gramas = product.Gramas,
            Kcal = product.Kcal,
            Categoria = product.Categoria,
        };
    }
}
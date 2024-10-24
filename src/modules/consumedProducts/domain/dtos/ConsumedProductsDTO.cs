using System.ComponentModel.DataAnnotations;
using Kcal.src.modules.consumedProducts.domain.models;

namespace Kcal.src.modules.consumedProducts.domain.dtos;

public class ConsumedProductsDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? Marca { get; set; }
    public int? Gramas { get; set; }
    public int Kcal { get; set; }
    public string Categoria { get; set; } = null!;
    public int Quantidade { get; set; }
    [DataType(DataType.Date)]
    public DateTime DataConsumo { get; set; }
















    public static ConsumedProductsDTO ModelToDto(ConsumedProducts cp)
    {
        return new ConsumedProductsDTO
        {
            ProductId = cp.ProductId,
            ProductName = cp.Product.Nome,
            Marca = cp.Product.Marca,
            Gramas = cp.Product.Gramas,
            Kcal = cp.Product.Kcal,
            Categoria = cp.Product.Categoria,
            Quantidade = cp.Quantidade,
            DataConsumo = cp.DataConsumo
        };
    }
}
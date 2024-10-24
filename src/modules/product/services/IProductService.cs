using Kcal.src.modules.product.domain.models;

namespace Kcal.src.modules.product.services;

public interface IProductService
{
    Product GetOne(Guid productId);
    IEnumerable<Product> GetAll();
    void Create(Product product);
    void Update(Product product);
    void Delete(Guid productId);
}
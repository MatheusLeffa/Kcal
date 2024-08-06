using Kcal.App.Models;

namespace Kcal.App.Services;

public interface IProductService
{
    Product GetOne(Guid productId);
    IEnumerable<Product> GetAll();
    void Create(Product product);
    void Update(Product product);
    void Delete(Guid productId);
}
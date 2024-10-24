using Kcal.src.modules.consumedProducts.domain.models;

namespace Kcal.src.modules.consumedProducts.repositories;

public interface IConsumedProductsRepository
{
    Task<List<ConsumedProducts>> GetAll();
    Task<ConsumedProducts?> GetByUser(Guid userId);
}
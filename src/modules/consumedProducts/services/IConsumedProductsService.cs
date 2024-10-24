using Kcal.src.modules.consumedProducts.domain.dtos;

namespace Kcal.src.modules.consumedProducts.services;

public interface IConsumedProductsService
{
    Task<List<ConsumedProductsDTO>> GetAll();
    Task<ConsumedProductsDTO?> GetByUser(Guid userId);
}
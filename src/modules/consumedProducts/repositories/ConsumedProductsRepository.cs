using Kcal.src.infrastructure.database;
using Kcal.src.modules.consumedProducts.domain.models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.src.modules.consumedProducts.repositories;

public class ConsumedProductsRepository(ApplicationContext appContext) : IConsumedProductsRepository
{
    private readonly ApplicationContext _appContext = appContext;

    public Task<List<ConsumedProducts>> GetAll()
    {
        return _appContext.ConsumedProducts
            .Include(cp => cp.Product)
            .Include(cp => cp.User)
            .OrderBy(cp => cp.Product.Nome)
            .ToListAsync();
    }

    public Task<ConsumedProducts?> GetByUser(Guid userId)
    {
        return _appContext.ConsumedProducts
            .Include(cp => cp.Product)
            .Include(cp => cp.User)
            .OrderBy(cp => cp.Product.Nome)
            .FirstOrDefaultAsync(cp => cp.UserId == userId);
    }
}
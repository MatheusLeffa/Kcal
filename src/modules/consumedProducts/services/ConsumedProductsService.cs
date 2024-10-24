using Kcal.src.Exceptions;
using Kcal.src.modules.consumedProducts.domain.dtos;
using Kcal.src.modules.consumedProducts.domain.models;
using Kcal.src.modules.consumedProducts.repositories;

namespace Kcal.src.modules.consumedProducts.services;

public class ConsumedProductsService(IConsumedProductsRepository consumedProductsRepository) : IConsumedProductsService
{
    private readonly IConsumedProductsRepository _consumedProductsRepository = consumedProductsRepository;

    public async Task<List<ConsumedProductsDTO>> GetAll()
    {
        List<ConsumedProducts> consumedProducts = await _consumedProductsRepository.GetAll();
        return consumedProducts.Select(cp => ConsumedProductsDTO.ModelToDto(cp)).ToList();
    }

    public async Task<ConsumedProductsDTO?> GetByUser(Guid userId)
    {
        var consumedProducts = await _consumedProductsRepository.GetByUser(userId) ?? throw new NotFoundException("Nenhum registro encontrado!");
        return ConsumedProductsDTO.ModelToDto(consumedProducts);
    }
}
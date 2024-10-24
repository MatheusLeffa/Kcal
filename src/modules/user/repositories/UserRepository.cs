using Microsoft.EntityFrameworkCore;
using Kcal.src.Exceptions;
using Kcal.src.infrastructure.database;
using Kcal.src.modules.user.domain.models;

namespace Kcal.src.modules.user.repositories;

public class UserRepository(ApplicationContext context) : IUserRepository
{
    private readonly ApplicationContext _dbContext = context;

    public async Task<List<User>> GetAll()
    {
        return await _dbContext.Users
            .Include(u => u.ConsumedProducts)
            .ThenInclude(cp => cp.Product)
            .OrderBy(user => user.Name)
            .ToListAsync();
    }

    public async Task<User?> GetById(Guid userId)
    {
        return await _dbContext.Users
            .Include(u => u.ConsumedProducts)
            .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<List<User>> GetByName(string name)
    {
        return await _dbContext.Users
            .Where(user => user.Name.Contains(name))
            .OrderBy(user => user.Name)
            .ToListAsync();
    }

    public async Task Create(User newUser)
    {
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid userId)
    {
        User? user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundException("Não foi localizado usuário!");
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsEmailAvaliable(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
}
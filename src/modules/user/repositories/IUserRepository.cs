using Kcal.src.modules.user.domain.models;

namespace Kcal.src.modules.user.repositories;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> GetById(Guid userId);
    Task<List<User>> GetByName(string name);
    Task Create(User newUser);
    Task Update();
    Task Delete(Guid userId);
    Task<bool> IsEmailAvaliable(string email);
}
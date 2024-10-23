using Kcal.App.DTOs;
using Kcal.App.Models;

namespace Kcal.App.Repository;

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
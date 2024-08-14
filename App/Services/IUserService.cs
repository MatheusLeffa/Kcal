using Kcal.App.DTOs;
using Kcal.App.Models;

namespace Kcal.App.Services;

public interface IUserService
{
    Task<List<UserDTO?>> GetAll();
    Task<UserDTO?> GetById(Guid userId);
    Task<List<UserDTO?>> GetByName(string name);
    Task<UserDTO> Create(User newUser);
    Task<bool> Update(Guid userId, UpdateUserDTO updatedUser);
    Task<bool> UpdateCredencials(Guid userId, UpdateUserCredencialsDTO updatedUser);
    Task<bool> Delete(Guid userId);
    Task<bool> IsEmailNotAvaliable(string email);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
}
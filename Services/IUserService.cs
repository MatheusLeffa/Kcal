using Kcal.DTOs;
using Kcal.Models;

namespace Kcal.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO?>> GetAll();
    Task<UserDTO?> GetById(Guid userId);
    Task<IEnumerable<UserDTO?>> GetByName(string name);
    Task<UserDTO> Create(User newUser);
    Task<bool> Update(Guid userId, UpdateUserDTO updatedUser);
    Task<bool> UpdateCredencials(Guid userId, UpdateUserCredencialsDTO updatedUser);
    Task<bool> Delete(Guid userId);
    Task<bool> IsEmailNotAvaliable(string email);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
}
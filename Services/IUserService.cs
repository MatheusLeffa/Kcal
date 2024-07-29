using Kcal.DTOs;
using Kcal.Models;

namespace Kcal.Services;

public interface IUserService
{
    Task<UserDTO?> GetOne(Guid userId);
    Task<IEnumerable<UserDTO?>> GetAll();
    Task<UserDTO> Create(User newUser);
    Task<bool> Update(Guid userId, UpdateUserDTO updatedUser);
    Task<bool> UpdateCredencials(Guid userId, UpdateUserCredencialsDTO updatedUser);
    Task<bool> Delete(Guid userId);
    Task<bool> IsEmailNotAvaliable(string email);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
}
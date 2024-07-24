using Kcal.DTOs;
using Kcal.Models;

namespace Kcal.Services;

public interface IUserService
{
    Task<UserDTO?> GetOne(Guid userId);
    Task<IEnumerable<UserDTO?>> GetAll();
    Task<UserDTO> Create(User newUser);
    Task<bool> Update(UpdateUserDTO updatedUser);
    Task<bool> UpdateCredencials(UpdateUserCredencialsDTO updatedUser);
    Task<bool> Delete(Guid userId);
    Task<bool> IsEmailAvaliable(string email);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
}
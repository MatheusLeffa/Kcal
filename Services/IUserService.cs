using Kcal.DTOs;
using Kcal.Models;

namespace Kcal.Services;

public interface IUserService
{
    Task<UserDTO?> GetOne(Guid userId);
    Task<IEnumerable<UserDTO?>> GetAll();
    Task<UserDTO> Create(User newUser);
    Task<UserDTO?> Update(User updatedUser);
    Task<UserDTO?> UpdateCredencials(User updatedUser);
    Task<UserDTO?> Delete(Guid userId);
    Task<bool> ConsultaSeExiste(Guid id);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
}
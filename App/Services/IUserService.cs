using Kcal.App.DTOs;
using Kcal.App.Models;

namespace Kcal.App.Services;

public interface IUserService
{
    Task<List<UserDTO?>> GetAll();
    Task<UserDTO?> GetById(Guid userId);
    Task<List<UserDTO?>> GetByName(string name);
    Task<UserDTO> Create(User newUser);
    Task<UserDTO> Update(Guid userId, UpdateUserDTO updatedUser);
    Task<UserDTO> UpdateCredencials(Guid userId, UpdateUserCredencialsDTO updatedUser);
    Task Delete(Guid userId);
    bool IsEmailNotAvaliable(string email);
}
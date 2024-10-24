using Kcal.src.modules.user.domain.dtos;
using Kcal.src.modules.user.domain.models;

namespace Kcal.src.modules.user.services;

public interface IUserService
{
    Task<List<UserDTO?>> GetAll();
    Task<UserDTO?> GetById(Guid userId);
    Task<List<UserDTO?>> GetByName(string name);
    Task<UserDTO> Create(User newUser);
    Task<UserDTO> Update(Guid userId, UpdateUserDTO updatedUser);
    Task Delete(Guid userId);
}
using Kcal.App.DTOs;
using Kcal.App.Exceptions;
using Kcal.App.Models;
using Kcal.App.Repository;

namespace Kcal.App.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<UserDTO?>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();
        return users.Select(user => UserDTO.ModelToDto(user)).ToList();
    }

    public async Task<UserDTO?> GetById(Guid userId)
    {
        User user = await _userRepository.GetById(userId) ?? throw new NotFoundException("Não foi localizado usuário!");
        return UserDTO.ModelToDto(user);
    }

    public async Task<List<UserDTO?>> GetByName(string name)
    {
        var users = await _userRepository.GetByName(name);

        if (users.Count == 0)
            throw new NotFoundException("Não foi localizado usuários!");

        return users.Select(user => UserDTO.ModelToDto(user)).ToList();
    }


    public async Task<UserDTO> Create(User newUser)
    {
        if (await IsEmailNotAvaliable(newUser.Email)) throw new EmailNotAvaliableException("Email já em uso!");

        newUser.DataCadastro = DateTime.Now;
        newUser.MetabolismoBasal = CalcularMetabolismoBasal(newUser);

        await _userRepository.Create(newUser);
        return UserDTO.ModelToDto(newUser)!;
    }

    public async Task<UserDTO> Update(Guid userId, UpdateUserDTO updatedUser)
    {
        User? user = await _userRepository.GetById(userId) ?? throw new NotFoundException("Não foi localizado usuário!");

        if (updatedUser.Name != null)
            user.Name = updatedUser.Name;

        if (updatedUser.DataNascimento != null)
            user.DataNascimento = (DateTime)updatedUser.DataNascimento;

        if (updatedUser.Sexo != null)
            user.Sexo = updatedUser.Sexo;

        if (updatedUser.Peso != null)
            user.Peso = (int)updatedUser.Peso;

        if (updatedUser.Altura != null)
            user.Altura = (int)updatedUser.Altura;

        user.MetabolismoBasal = CalcularMetabolismoBasal(user);

        await _userRepository.Update();
        return UserDTO.ModelToDto(user)!;
    }

    public async Task Delete(Guid userId)
    {
        await _userRepository.Delete(userId);
    }

    public async Task<bool> IsEmailNotAvaliable(string email)
    {
        return await _userRepository.IsEmailAvaliable(email);
    }

    private int CalcularMetabolismoBasal(User user)
    {
        int altura = user.Altura;
        int peso = user.Peso;
        int idade = CalcularIdade(user.DataNascimento);
        string sexo = user.Sexo;

        if (sexo.ToUpper() == "F")
        {
            return (10 * peso) + (6 * altura) - (5 * idade) - 161;
        }
        else
        {
            return (10 * peso) + (6 * altura) - (5 * idade) + 5;
        }
    }

    private static int CalcularIdade(DateTime dataNascimento)
    {
        int idade = DateTime.Now.Year - dataNascimento.Year;

        // Ajusta se a data de nascimento ainda não ocorreu neste ano
        if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
        {
            idade--;
        }
        return idade;
    }
}
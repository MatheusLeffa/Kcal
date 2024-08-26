using Kcal.App.Database;
using Kcal.App.DTOs;
using Kcal.App.Exceptions;
using Kcal.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.App.Services;

public class UserService(Context dbContext) : IUserService
{
    private readonly Context _dbContext = dbContext;

    public async Task<List<UserDTO?>> GetAll()
    {
        return await _dbContext.Users
            .Include(u => u.ConsumedProducts)
            .ThenInclude(cp => cp.Product)
            .OrderBy(user => user.Name)
            .Select(user => UserDTO.ModelToDto(user))
            .ToListAsync();
    }

    public async Task<UserDTO?> GetById(Guid userId)
    {
        User? user = await _dbContext.Users
            .Include(u => u.ConsumedProducts)
            .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new NotFoundException("Não foi localizado usuário!");
        return UserDTO.ModelToDto(user);
    }

    public async Task<List<UserDTO?>> GetByName(string name)
    {
        var users = await _dbContext.Users
            .Where(user => user.Name.Contains(name))
            .OrderBy(user => user.Name)
            .Select(user => UserDTO.ModelToDto(user))
            .ToListAsync();

        return users;
    }


    public async Task<UserDTO> Create(User newUser)
    {
        if (IsEmailNotAvaliable(newUser.Email)) throw new EmailNotAvaliableException("Email já em uso!");

        newUser.DataCadastro = DateTime.Now;
        newUser.MetabolismoBasal = CalcularMetabolismoBasal(newUser);

        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        return UserDTO.ModelToDto(newUser)!;
    }

    public async Task<UserDTO> Update(Guid userId, UpdateUserDTO updatedUser)
    {
        User? user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundException("Não foi localizado usuário!");

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

        await _dbContext.SaveChangesAsync();
        return UserDTO.ModelToDto(user)!;
    }

    public async Task<UserDTO> UpdateCredencials(Guid userId, UpdateUserCredencialsDTO updatedUser)
    {

        User? user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundException("Não foi localizado usuário!");

        if (IsEmailNotAvaliable(updatedUser.EmailNovo)) throw new EmailNotAvaliableException("Email já em uso!");

        user.Email = updatedUser.EmailNovo;
        user.Senha = updatedUser.SenhaNova;
        await _dbContext.SaveChangesAsync();
        return UserDTO.ModelToDto(user)!;
    }

    public async Task Delete(Guid userId)
    {
        User? user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundException("Não foi localizado usuário!");
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public bool IsEmailNotAvaliable(string email)
    {
        return _dbContext.Users.Any(u => u.Email == email);
    }

    private int CalcularMetabolismoBasal(User user)
    {
        int altura = user.Altura;
        int peso = user.Peso;
        int idade = CalcularIdade(user.DataNascimento);
        string sexo = user.Sexo;

        if (sexo == "f" || sexo == "F")
        {
            return (10 * peso) + (6 * altura) - (5 * idade) - 161;
        }
        else
        {
            return (10 * peso) + (6 * altura) - (5 * idade) + 5;
        }
    }

    private int CalcularIdade(DateTime dataNascimento)
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
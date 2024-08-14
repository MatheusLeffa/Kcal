using Kcal.App.Database;
using Kcal.App.DTOs;
using Kcal.App.Exceptions;
using Kcal.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.App.Services;

public class UserService(Context dbContext) : IUserService
{
    private readonly Context _dbContext = dbContext;

    private const string ERRO_SALVAR_ALTERACOES = "Erro ao salvar alterações no banco de dados";


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

        if (users.Count == 0)
            throw new NotFoundException("Não foi localizado usuários!");

        return users;
    }


    public async Task<UserDTO> Create(User newUser)
    {
        newUser.DataCadastro = DateTime.Now;
        newUser.MetabolismoBasal = CalcularMetabolismoBasal(newUser);

        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        return UserDTO.ModelToDto(newUser)!;


    }

    public async Task<bool> Update(Guid userId, UpdateUserDTO updatedUser)
    {
        User? user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return false;

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

        return await TryToSaveAsync();
    }

    public async Task<bool> UpdateCredencials(Guid userId, UpdateUserCredencialsDTO updatedUser)
    {
        User? user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return false;

        user.Email = updatedUser.Email;
        user.Senha = updatedUser.Senha;

        return await TryToSaveAsync();
    }

    public async Task<bool> Delete(Guid userId)
    {
        User? user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return false;

        _dbContext.Users.Remove(user);

        return await TryToSaveAsync();
    }

    public async Task<bool> IsEmailNotAvaliable(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
    {
        User? user = await _dbContext.Users
            .Where(user => user.Email == email && user.Senha == password)
            .FirstOrDefaultAsync();
        return user != null;
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

    private async Task<bool> TryToSaveAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new DbUpdateException(ERRO_SALVAR_ALTERACOES, ex);
        }
    }
}
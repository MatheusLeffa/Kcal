using Kcal.Database;
using Kcal.DTOs;
using Kcal.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.Services;

public class UserService(Context dbContext) : IUserService
{
    private readonly Context dbContext = dbContext;

    public async Task<IEnumerable<UserDTO?>> GetAll()
    {
        return await dbContext.Users
            .Include(u => u.ConsumedProducts)
            .ThenInclude(cp => cp.Product)
            .OrderBy(user => user.Name)
            .Select(user => UserDTO.ModelToDto(user))
            .ToListAsync();
    }

    public async Task<UserDTO?> GetOne(Guid userId)
    {
        try
        {
            User? user = await dbContext.Users
                .Include(u => u.ConsumedProducts)
                .ThenInclude(cp => cp.Product)
                .FirstAsync(u => u.UserId == userId);

            return UserDTO.ModelToDto(user);
        }
        catch (Exception e)
        {
            throw new OperationCanceledException($"Erro ao consultar usuário!", e);
        }
    }

    public async Task<UserDTO> Create(User newUser)
    {
        try
        {
            newUser.DataCadastro = DateTime.Now;
            newUser.MetabolismoBasal = CalcularMetabolismoBasal(newUser);

            await dbContext.Users.AddAsync(newUser);
            await dbContext.SaveChangesAsync();
            return UserDTO.ModelToDto(newUser)!;
        }
        catch (Exception e)
        {
            throw new OperationCanceledException($"Erro ao criar usuário!", e);
        }

    }

    public async Task<UserDTO?> Update(User updatedUser)
    {
        User? user = await dbContext.Users.FindAsync(updatedUser.UserId);
        if (user != null)
        {
            user.Name = updatedUser.Name;

            if (user.DataNascimento != updatedUser.DataNascimento ||
            user.Sexo != updatedUser.Sexo ||
            user.Peso != updatedUser.Peso ||
            user.Altura != updatedUser.Altura)
            {
                user.DataNascimento = updatedUser.DataNascimento;
                user.Sexo = updatedUser.Sexo;
                user.Peso = updatedUser.Peso;
                user.Altura = updatedUser.Altura;
                user.MetabolismoBasal = CalcularMetabolismoBasal(user);
            }

            await dbContext.SaveChangesAsync();
        }
        return UserDTO.ModelToDto(user);
    }

    public async Task<UserDTO?> UpdateCredencials(User updatedUser)
    {
        User? user = await dbContext.Users.FindAsync(updatedUser.UserId);
        if (user != null)
        {
            user.Email = updatedUser.Email;
            user.Senha = updatedUser.Senha;
            await dbContext.SaveChangesAsync();
        }
        return UserDTO.ModelToDto(user);
    }

    public async Task<UserDTO?> Delete(Guid userId)
    {
        User? user = await dbContext.Users.FindAsync(userId);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        return UserDTO.ModelToDto(user);
    }

    public async Task<bool> IsEmailAvaliable(string email)
    {
        return await dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
    {
        User? user = await dbContext.Users
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
}
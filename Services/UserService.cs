using Kcal.Database;
using Kcal.DTOs;
using Kcal.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.Services;

public class UserService(Context dbContext) : IUserService
{
    private readonly Context dbContext = dbContext;

    private const string ERRO_SALVAR_ALTERACOES = "Erro ao salvar alterações no banco de dados";


    public async Task<IEnumerable<UserDTO?>> GetAll()
    {
        try
        {
            return await dbContext.Users
                .Include(u => u.ConsumedProducts)
                .ThenInclude(cp => cp.Product)
                .OrderBy(user => user.Name)
                .Select(user => UserDTO.ModelToDto(user))
                .ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            throw new OperationCanceledException("Erro ao consultar usuários!", ex);
        }
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
        catch (OperationCanceledException ex)
        {
            throw new OperationCanceledException("Erro ao consultar usuário!", ex);
        }
    }

    public async Task<UserDTO> Create(User newUser)
    {
        newUser.DataCadastro = DateTime.Now;
        newUser.MetabolismoBasal = CalcularMetabolismoBasal(newUser);

        try
        {
            await dbContext.Users.AddAsync(newUser);
            await dbContext.SaveChangesAsync();
            return UserDTO.ModelToDto(newUser)!;
        }
        catch (OperationCanceledException ex)
        {
            throw new OperationCanceledException("Erro ao criar usuário!", ex);
        }
    }

    public async Task<bool> Update(UpdateUserDTO updatedUser)
    {
        User? user = await dbContext.Users.FindAsync(updatedUser.UserId);
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

    public async Task<bool> UpdateCredencials(UpdateUserCredencialsDTO updatedUser)
    {
        User? user = await dbContext.Users.FindAsync(updatedUser.UserId);
        if (user == null) return false;

        user.Email = updatedUser.Email;
        user.Senha = updatedUser.Senha;

        return await TryToSaveAsync();
    }

    public async Task<bool> Delete(Guid userId)
    {
        User? user = await dbContext.Users.FindAsync(userId);
        if (user == null) return false;

        dbContext.Users.Remove(user);

        return await TryToSaveAsync();
    }

    private async Task<bool> TryToSaveAsync()
    {
        try
        {
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException(ERRO_SALVAR_ALTERACOES, ex);
        }
    }

    public async Task<bool> IsEmailAvaliable(string email)
    {
        return !await dbContext.Users.AnyAsync(u => u.Email == email);
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
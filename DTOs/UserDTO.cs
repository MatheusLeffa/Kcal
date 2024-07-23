using Kcal.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Kcal.DTOs;

public class UserDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DataNascimento { get; set; }
    public string Sexo { get; set; } = null!;
    public int Peso { get; set; }
    public int Altura { get; set; }
    public int MetabolismoBasal { get; set; }
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public IEnumerable<ConsumedProductsDTO> ConsumedProducts { get; set; } = null!;

    public static User DtoToModel(UserDTO userDto)
    {
        return new User
        {
            UserId = userDto.UserId,
            Name = userDto.Name,
            DataNascimento = userDto.DataNascimento,
            Sexo = userDto.Sexo,
            Peso = userDto.Peso,
            Altura = userDto.Altura,
            Email = userDto.Email,
            Senha = userDto.Senha,
        };
    }

    public static UserDTO ModelToDto(User? user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            DataNascimento = user.DataNascimento,
            Sexo = user.Sexo,
            Peso = user.Peso,
            Altura = user.Altura,
            MetabolismoBasal = user.MetabolismoBasal,
            Email = user.Email,
            ConsumedProducts = user.ConsumedProducts.Select(ConsumedProductsDTO.ModelToDto)
        };
    }
}

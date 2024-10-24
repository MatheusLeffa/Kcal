using System.ComponentModel.DataAnnotations;
using Kcal.src.modules.user.domain.models;


namespace Kcal.src.modules.user.domain.dtos;

public class UserDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }
    public string Sexo { get; set; } = null!;
    public int Peso { get; set; }
    public int Altura { get; set; }
    public int MetabolismoBasal { get; set; }
    [EmailAddress]
    public string Email { get; set; } = null!;

    public static User DtoToModel(UserDTO userDto)
    {
        return new User
        {
            Id = userDto.UserId,
            Name = userDto.Name,
            DataNascimento = userDto.DataNascimento,
            Sexo = userDto.Sexo,
            Peso = userDto.Peso,
            Altura = userDto.Altura,
            Email = userDto.Email,
        };
    }

    public static UserDTO? ModelToDto(User? user)
    {
        if (user == null) return null;

        return new UserDTO
        {
            UserId = user.Id,
            Name = user.Name,
            DataNascimento = user.DataNascimento,
            Sexo = user.Sexo,
            Peso = user.Peso,
            Altura = user.Altura,
            MetabolismoBasal = user.MetabolismoBasal,
            Email = user.Email,
        };
    }
}

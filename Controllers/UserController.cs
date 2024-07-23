namespace Kcal.Controllers;

using Kcal.DTOs;
using Kcal.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
    {
        UserDTO? user = await _userService.GetOne(id);
        if (user == null)
            return NotFound("Usuário não localizado!");

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create(CreateUserDTO userDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        bool isEmailAvaliable = await _userService.IsEmailAvaliable(userDto.Email);
        if (!isEmailAvaliable)
        {
            UserDTO createdUser = await _userService.Create(CreateUserDTO.DtoToModel(userDto));
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }
        return BadRequest("E-mail já em uso!");
    }
}

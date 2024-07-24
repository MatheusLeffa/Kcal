namespace Kcal.Controllers;

using Kcal.DTOs;
using Kcal.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserService> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserService> logger = logger;

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
    {
        UserDTO? user = await _userService.GetOne(id);
        if (user == null)
            return NotFound("Usuário não localizado!");

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO userDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        bool isEmailAvaliable = await _userService.IsEmailAvaliable(userDto.Email);
        if (isEmailAvaliable)
        {
            UserDTO createdUser = await _userService.Create(CreateUserDTO.DtoToModel(userDto));
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }
        return BadRequest("E-mail já em uso!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO userDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            bool isUpdated = await _userService.Update(userDto);
            return isUpdated ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a atualização do usuário.");
        }
    }
}

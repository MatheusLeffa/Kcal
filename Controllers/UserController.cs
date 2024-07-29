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


    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers(Guid id)
    {
        IEnumerable<UserDTO?> users = await _userService.GetAll();

        if (!users.Any())
            return NotFound("Não existe usuários cadastrados.");

        return Ok(users);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
    {
        UserDTO? user = await _userService.GetById(id);
        if (user == null)
            return NotFound("Usuário não localizado!");

        return Ok(user);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> SearchUsersByName([FromQuery] string name)
    {
        IEnumerable<UserDTO?> users = await _userService.GetByName(name);

        if (users == null || !users.Any())
            return NotFound($"Não foi localizado usuários com o nome '{name}'.");

        return Ok(users);
    }


    [HttpPost("Create")]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool isEmailNotAvaliable = await _userService.IsEmailNotAvaliable(userDto.Email);
        if (isEmailNotAvaliable)
            return BadRequest("E-mail já em uso!");

        try
        {
            UserDTO createdUser = await _userService.Create(CreateUserDTO.DtoToModel(userDto));
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar usuário.");
        }
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDTO userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            bool isUpdated = await _userService.Update(id, userDto);
            return isUpdated ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao salvar alterações do usuário no banco de dados.");
        }
    }

    [HttpPut("UpdateCredencials/{id}")]
    public async Task<IActionResult> UpdateUserCredencials(Guid id, UpdateUserCredencialsDTO userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool isEmailNotAvaliable = await _userService.IsEmailNotAvaliable(userDto.Email);
        if (isEmailNotAvaliable)
            return BadRequest("E-mail já em uso!");

        try
        {
            bool isUpdated = await _userService.UpdateCredencials(id, userDto);
            return isUpdated ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao alterar as credenciais do usuário no banco.");
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            bool isDeleted = await _userService.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar usuário no banco de dados.");
        }
    }
}

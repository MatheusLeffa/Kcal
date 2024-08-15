namespace Kcal.App.Controllers;

using System.Data;
using Kcal.App.DTOs;
using Kcal.App.Exceptions;
using Kcal.App.Services;
using Kcal.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserService> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserService> logger = logger;


    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAll(Guid id)
    {
        try
        {
            List<UserDTO?> users = await _userService.GetAll();
            return Ok(new SucessoDto<List<UserDTO?>>(users));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro ao buscar usuários."));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById(Guid id)
    {
        try
        {
            UserDTO? user = await _userService.GetById(id);
            return Ok(new SucessoDto<UserDTO>(user));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErroDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro ao buscar usuário."));
        }
    }

    [HttpGet("Search")]
    public async Task<ActionResult<List<UserDTO>>> GetByName([FromQuery] string name)
    {
        try
        {
            List<UserDTO?> users = await _userService.GetByName(name);
            return Ok(new SucessoDto<List<UserDTO?>>(users));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro ao buscar usuário."));
        }
    }

    [HttpPost("Create")]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO userDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            UserDTO createdUser = await _userService.Create(CreateUserDTO.DtoToModel(userDto));
            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, new SucessoDto<UserDTO>(createdUser));
        }
        catch (EmailNotAvaliableException ex)
        {
            return BadRequest(new ErroDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro ao criar usuário."));
        }
    }

    [HttpPut("Update/{id:guid}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserDTO userDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedUser = await _userService.Update(id, userDto);
            return AcceptedAtAction(nameof(GetById), new { id = updatedUser.UserId }, new SucessoDto<UserDTO>(updatedUser));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErroDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao salvar alterações do usuário no banco de dados.");
        }
    }

    [HttpPut("UpdateCredencials/{id:guid}")]
    public async Task<IActionResult> UpdateCredencials([FromRoute] Guid id, [FromBody] UpdateUserCredencialsDTO userDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            UserDTO user = await _userService.UpdateCredencials(id, userDto);
            return AcceptedAtAction(nameof(GetById), new { id = user.UserId }, new SucessoDto<UserDTO>(user));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErroDto(ex.Message));
        }
        catch (EmailNotAvaliableException ex)
        {
            return BadRequest(new ErroDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao alterar as credenciais do usuário no banco.");
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        try
        {
            await _userService.Delete(id);
            return AcceptedAtAction(nameof(GetById), new { id });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErroDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar usuário no banco de dados.");
        }
    }
}

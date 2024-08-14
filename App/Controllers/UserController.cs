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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro interno no servidor."));
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro interno no servidor."));
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
        catch (NotFoundException ex)
        {
            return NotFound(new ErroDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErroDto("Erro interno no servidor."));
        }
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
            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, createdUser);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar usuário no banco de dados.");
        }
    }
}

namespace Kcal.src.modules.user.controllers;

using Kcal.src.Exceptions;
using Kcal.src.modules.user.domain.dtos;
using Kcal.src.modules.user.services;
using Kcal.src.utils;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserService> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserService> logger = logger;


    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAll()
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

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        try
        {
            await _userService.Delete(id);
            return Ok("Usuário deletado com sucesso!");
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

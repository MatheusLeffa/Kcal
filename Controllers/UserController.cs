namespace Kcal.Controllers;

using Kcal.DTOs;
using Kcal.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService userService = userService;

    // [HttpGet]
    // public async Task<IActionResult> Get()
    // {

    //     return Ok();
    // }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create(UserDTO userDto)
    {
        bool UserExists = await userService.ConsultaSeExiste(userDto.UserId);
        if (!UserExists) return BadRequest("User doesn't exist!");

        return 
    }
}

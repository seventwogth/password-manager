using Microsoft.AspNetCore.Mvc;
using PManager.API.Token;
using PManager.API.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JWTService _tokenService;

    public AuthController(JWTService tokenService)
    {
        _tokenService = tokenService;
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        if (string.IsNullOrEmpty(model.UserName))
        {
            return BadRequest("Username is required.");
        }

        var token = _tokenService.GenerateToken(model.UserName);
        return Ok(new { Token = token });
    }

}

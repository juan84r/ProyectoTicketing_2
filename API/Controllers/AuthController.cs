using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Auth;

namespace API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    private readonly RegisterHandler _registerHandler;

    public AuthController(LoginHandler loginHandler,RegisterHandler registerHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _loginHandler.Handle(request);

        if (user == null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        return Ok(new
        {
            message = "Login exitoso",
            userId = user.Id,
            email = user.Email,
            role = user.Role
        });
    }
    [HttpPost("register")]

    public async Task<IActionResult> Register(RegisterRequest request)
    {
    var result = await _registerHandler.Handle(request);

    if (!result)
        return BadRequest("El usuario ya existe");

    return Ok("Usuario creado");
    }

}
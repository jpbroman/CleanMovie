using CleanMovie.API.Models;
using CleanMovie.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanMovie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            await _authService.RegisterAsync(
                request.Username,
                request.Password,
                request.Role);

            return Ok(new
            {
                Message = "User registered successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(
            request.Username,
            request.Password);

        if (token == null)
            return Unauthorized(new
            {
                Message = "Invalid username or password."
            });

        return Ok(new
        {
            Token = token
        });
    }
}


using Microsoft.AspNetCore.Mvc;
using PedidoApi.Models.Dtos;
using PedidoApi.Services;

namespace PedidoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login(LoginDto dto)
    {
        return Ok(_authService.Login(dto));
    }

    [HttpPost("register")]
    public ActionResult<AuthResponseDto> Register(RegisterDto dto)
    {
        return Ok(_authService.Register(dto));
    }

    [HttpPost("google")]
    public async Task<ActionResult<AuthResponseDto>> Google(GoogleLoginDto dto)
    {
        return Ok(await _authService.GoogleLogin(dto));
    }
}

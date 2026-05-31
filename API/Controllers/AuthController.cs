using API.Models.DTOs;
using API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthService Service { get; set; }

        public AuthController(AuthService service)
        {
            Service = service;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var result = Service.Login(dto);
                if (result == null)
                    return Unauthorized(new { message = "Usuario o contrasena incorrectos." });
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = string.Join("\n", ex.Errors.Select(x => x.ErrorMessage)) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public IActionResult Registrar(RegistroUsuarioDTO dto)
        {
            try
            {
                var result = Service.Registrar(dto);
                if (result == null)
                    return BadRequest(new { message = "El correo ya esta registrado." });
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = string.Join("\n", ex.Errors.Select(x => x.ErrorMessage)) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> Google(GoogleLoginDTO dto)
        {
            try
            {
                return Ok(await Service.GoogleLogin(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

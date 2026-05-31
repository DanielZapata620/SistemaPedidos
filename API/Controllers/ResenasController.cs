using API.Models.DTOs;
using API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ResenasController : ControllerBase
    {
        public ResenasController(ResenasService service, IWebHostEnvironment env, IValidator<CrearResenaDTO> crearValidator, IValidator<EditarResenaDTO> editarValidator)
        {
            Service = service;
            Env = env;
            CrearValidator = crearValidator;
            EditarValidator = editarValidator;
        }

        public ResenasService Service { get; }
        public IWebHostEnvironment Env { get; }
        public IValidator<CrearResenaDTO> CrearValidator { get; }
        public IValidator<EditarResenaDTO> EditarValidator { get; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetResenas()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);

            var resenas = Service.ObtenerResenas(idUsuario);
            return Ok(resenas);
        }

        [HttpGet("misresenas")]
        public IActionResult GetMisResenas()
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
            var resenas = Service.ObtenerMisResenas(idUsuario);
            return Ok(resenas);
        }

        [HttpPost]
        public IActionResult CrearResena(CrearResenaDTO dto)
        {
            try
            {
                var validacion = CrearValidator.Validate(dto);

                if (!validacion.IsValid)
                {
                    var errores = string.Join("\n",
                        validacion.Errors.Select(e => e.ErrorMessage));

                    return BadRequest(errores);
                }

                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                if (idUsuario == 0)
                    return Unauthorized();

                var uploadsPath = ObtenerUploadsPath();
                Service.CrearResena(dto, idUsuario, uploadsPath);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditarResena(EditarResenaDTO dto)
        {
            try
            {
                var validacion = EditarValidator.Validate(dto);

                if (!validacion.IsValid)
                {
                    var errores = string.Join("\n",
                        validacion.Errors.Select(e => e.ErrorMessage));

                    return BadRequest(errores);
                }

                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                if (idUsuario == 0)
                    return Unauthorized();

                var uploadsPath = ObtenerUploadsPath();
                Service.EditarResena(dto, idUsuario, uploadsPath);

              

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarResena(int id)
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
            if (idUsuario == 0)
                return Unauthorized();

            var uploadsPath = ObtenerUploadsPath();
            Service.EliminarResena(id, idUsuario, uploadsPath);

       

            return Ok("Resena eliminada correctamente");
        }

        [HttpPut("{id}/like")]
        public IActionResult Like(int id)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                if (idUsuario == 0)
                    return Unauthorized();

                Service.Like(id, idUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/dislike")]
        public IActionResult Dislike(int id)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                if (idUsuario == 0)
                    return Unauthorized();

                Service.Dislike(id, idUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ObtenerUploadsPath()
        {
            var webRoot = Env.WebRootPath ?? Path.Combine(Env.ContentRootPath, "wwwroot");
            return Path.Combine(webRoot, "Uploads");
        }
    }
}

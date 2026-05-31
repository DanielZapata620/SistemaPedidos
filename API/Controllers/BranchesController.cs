using API.Models.DTOs;
using API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        public SucursalesService Service { get; set; }

        public BranchesController(SucursalesService service)
        {
            Service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Service.Obtener());
        }

        [HttpPost]
        public IActionResult Post(CrearSucursalDTO dto)
        {
            try
            {
                return Ok(Service.Crear(dto));
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, EditarSucursalDTO dto)
        {
            try
            {
                var result = Service.Editar(id, dto);
                if (result == null) return NotFound(new { message = "Sucursal no encontrada." });
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Service.Eliminar(id))
                return NotFound(new { message = "Sucursal no encontrada." });
            return NoContent();
        }
    }
}

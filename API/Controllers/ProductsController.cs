using API.Models.DTOs;
using API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductosService Service { get; set; }

        public ProductsController(ProductosService service)
        {
            Service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Service.Obtener());
        }

        [HttpPost]
        public IActionResult Post(CrearProductoDTO dto)
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
        public IActionResult Put(int id, EditarProductoDTO dto)
        {
            try
            {
                var result = Service.Editar(id, dto);
                if (result == null) return NotFound(new { message = "Producto no encontrado." });
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
                return NotFound(new { message = "Producto no encontrado." });
            return NoContent();
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> Image(int id, [FromForm(Name = "image")] IFormFile image)
        {
            try
            {
                var result = await Service.SubirImagen(id, image);
                if (result == null) return NotFound(new { message = "Producto no encontrado." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

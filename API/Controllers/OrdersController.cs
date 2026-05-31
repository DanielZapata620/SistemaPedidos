using API.Models.DTOs;
using API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public PedidosService Service { get; set; }

        public OrdersController(PedidosService service)
        {
            Service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Service.ObtenerTodos());
        }

        [HttpGet("user/{id}")]
        public IActionResult UserOrders(int id)
        {
            return Ok(Service.ObtenerPorUsuario(id));
        }

        [HttpPost]
        public IActionResult Post(CrearPedidoDTO dto)
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

        [HttpPut("{id}/status")]
        public IActionResult Status(int id, EstadoPedidoDTO dto)
        {
            try
            {
                var result = Service.CambiarEstado(id, dto);
                if (result == null) return NotFound(new { message = "Pedido no encontrado." });
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
    }
}

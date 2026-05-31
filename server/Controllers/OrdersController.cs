using Microsoft.AspNetCore.Mvc;
using PedidoApi.Models.Dtos;
using PedidoApi.Services;

namespace PedidoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<List<OrderDto>> GetAll()
    {
        return Ok(_orderService.GetAll());
    }

    [HttpGet("user/{userId:int}")]
    public ActionResult<List<OrderDto>> GetByUser(int userId)
    {
        return Ok(_orderService.GetByUser(userId));
    }

    [HttpPost]
    public ActionResult<OrderDto> Create(OrderCreateDto dto)
    {
        return Ok(_orderService.Create(dto));
    }

    [HttpPut("{id:int}/status")]
    public ActionResult<OrderDto> UpdateStatus(int id, OrderStatusUpdateDto dto)
    {
        return Ok(_orderService.UpdateStatus(id, dto.Status));
    }
}

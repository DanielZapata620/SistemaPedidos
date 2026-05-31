using Microsoft.AspNetCore.Mvc;
using PedidoApi.Models.Dtos;
using PedidoApi.Services;

namespace PedidoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExternalController : ControllerBase
{
    private readonly ExternalInfoService _externalInfoService;

    public ExternalController(ExternalInfoService externalInfoService)
    {
        _externalInfoService = externalInfoService;
    }

    [HttpGet("store-info")]
    public async Task<ActionResult<StoreInfoDto>> GetStoreInfo()
    {
        return Ok(await _externalInfoService.GetStoreInfo());
    }
}

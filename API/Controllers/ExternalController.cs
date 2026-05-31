using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        public ExternalInfoService Service { get; set; }

        public ExternalController(ExternalInfoService service)
        {
            Service = service;
        }

        [HttpGet("store-info")]
        public async Task<IActionResult> Get()
        {
            return Ok(await Service.Obtener());
        }
    }
}

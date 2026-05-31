using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public DashboardService Service { get; set; }

        public DashboardController(DashboardService service)
        {
            Service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Service.Obtener());
        }
    }
}

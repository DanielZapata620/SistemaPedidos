using Microsoft.AspNetCore.Mvc;
using PedidoApi.Models.Dtos;
using PedidoApi.Services;

namespace PedidoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchesController : ControllerBase
{
    private readonly BranchService _branchService;

    public BranchesController(BranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public ActionResult<List<BranchDto>> GetAll()
    {
        return Ok(_branchService.GetAll());
    }

    [HttpPost]
    public ActionResult<BranchDto> Create(BranchCreateDto dto)
    {
        return Ok(_branchService.Create(dto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<BranchDto> Update(int id, BranchUpdateDto dto)
    {
        return Ok(_branchService.Update(id, dto));
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _branchService.Delete(id);
        return NoContent();
    }
}

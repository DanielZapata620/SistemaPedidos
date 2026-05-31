using Microsoft.AspNetCore.Mvc;
using PedidoApi.Models.Dtos;
using PedidoApi.Services;

namespace PedidoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<List<ProductDto>> GetAll()
    {
        return Ok(_productService.GetAll());
    }

    [HttpPost]
    public ActionResult<ProductDto> Create(ProductCreateDto dto)
    {
        return Ok(_productService.Create(dto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProductDto> Update(int id, ProductUpdateDto dto)
    {
        return Ok(_productService.Update(id, dto));
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _productService.Delete(id);
        return NoContent();
    }

    [HttpPost("{id:int}/image")]
    public async Task<ActionResult<ProductDto>> UploadImage(int id, IFormFile image)
    {
        return Ok(await _productService.UploadImage(id, image));
    }
}

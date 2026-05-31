using AutoMapper;
using FluentValidation;
using PedidoApi.Helpers;
using PedidoApi.Models.Dtos;
using PedidoApi.Models.Entities;
using PedidoApi.Repositories;

namespace PedidoApi.Services;

public class ProductService
{
    private readonly GenericRepository<ProductEntity> _products;
    private readonly IValidator<ProductCreateDto> _createValidator;
    private readonly IValidator<ProductUpdateDto> _updateValidator;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public ProductService(GenericRepository<ProductEntity> products, IValidator<ProductCreateDto> createValidator, IValidator<ProductUpdateDto> updateValidator, IMapper mapper, IWebHostEnvironment environment)
    {
        _products = products;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
        _environment = environment;
    }

    public List<ProductDto> GetAll()
    {
        return _products.GetAll().Select(_mapper.Map<ProductDto>).ToList();
    }

    public ProductDto Create(ProductCreateDto dto)
    {
        _createValidator.ValidateAndThrow(dto);
        var product = new ProductEntity
        {
            Name = SecurityHelper.Clean(dto.Name),
            Description = SecurityHelper.Clean(dto.Description),
            Price = dto.Price,
            ImageUrl = SecurityHelper.Clean(dto.ImageUrl),
            IsActive = true
        };

        return _mapper.Map<ProductDto>(_products.Add(product));
    }

    public ProductDto Update(int id, ProductUpdateDto dto)
    {
        _updateValidator.ValidateAndThrow(dto);
        var product = _products.GetById(id) ?? throw new KeyNotFoundException("Producto no encontrado.");
        product.Name = SecurityHelper.Clean(dto.Name);
        product.Description = SecurityHelper.Clean(dto.Description);
        product.Price = dto.Price;
        product.ImageUrl = SecurityHelper.Clean(dto.ImageUrl);
        product.IsActive = dto.IsActive;
        return _mapper.Map<ProductDto>(_products.Update(product));
    }

    public void Delete(int id)
    {
        if (!_products.Delete(id))
        {
            throw new KeyNotFoundException("Producto no encontrado.");
        }
    }

    public async Task<ProductDto> UploadImage(int id, IFormFile file)
    {
        var product = _products.GetById(id) ?? throw new KeyNotFoundException("Producto no encontrado.");
        if (file.Length == 0)
        {
            throw new InvalidOperationException("La imagen esta vacia.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension is not (".png" or ".jpg" or ".jpeg" or ".webp"))
        {
            throw new InvalidOperationException("Solo se permiten imagenes PNG, JPG o WEBP.");
        }

        var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var uploadPath = Path.Combine(webRoot, "uploads", "products");
        Directory.CreateDirectory(uploadPath);

        foreach (var oldFile in Directory.GetFiles(uploadPath, $"{id}.*"))
        {
            File.Delete(oldFile);
        }

        var fileName = $"{id}{extension}";
        var fullPath = Path.Combine(uploadPath, fileName);
        await using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream);
        }

        product.ImageUrl = $"/uploads/products/{fileName}";
        return _mapper.Map<ProductDto>(_products.Update(product));
    }
}

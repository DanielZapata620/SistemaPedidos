using API.Helpers;
using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
using AutoMapper;
using FluentValidation;

namespace API.Services
{
    public class ProductosService
    {
        public Repository<Producto> RepoProducto { get; set; }
        public Repository<DetallePedido> RepoDetalle { get; set; }
        public IValidator<CrearProductoDTO> CrearValidator { get; set; }
        public IValidator<EditarProductoDTO> EditarValidator { get; set; }
        public IMapper Mapper { get; set; }
        public IWebHostEnvironment Environment { get; set; }

        public ProductosService(Repository<Producto> repoProducto, Repository<DetallePedido> repoDetalle, IValidator<CrearProductoDTO> crearValidator, IValidator<EditarProductoDTO> editarValidator, IMapper mapper, IWebHostEnvironment environment)
        {
            RepoProducto = repoProducto;
            RepoDetalle = repoDetalle;
            CrearValidator = crearValidator;
            EditarValidator = editarValidator;
            Mapper = mapper;
            Environment = environment;
        }

        public List<ProductoDTO> Obtener()
        {
            return RepoProducto.GetAll().Select(x => Mapper.Map<ProductoDTO>(x)).ToList();
        }

        public ProductoDTO Crear(CrearProductoDTO dto)
        {
            CrearValidator.ValidateAndThrow(dto);
            var producto = new Producto
            {
                Nombre = SecurityHelper.Clean(dto.Name),
                Descripcion = SecurityHelper.Clean(dto.Description),
                Precio = dto.Price,
                Imagen = SecurityHelper.Clean(dto.ImageUrl),
                Activo = true
            };
            RepoProducto.Insert(producto);
            return Mapper.Map<ProductoDTO>(producto);
        }

        public ProductoDTO? Editar(int id, EditarProductoDTO dto)
        {
            EditarValidator.ValidateAndThrow(dto);
            var producto = RepoProducto.Get(id);
            if (producto == null) return null;
            producto.Nombre = SecurityHelper.Clean(dto.Name);
            producto.Descripcion = SecurityHelper.Clean(dto.Description);
            producto.Precio = dto.Price;
            producto.Imagen = SecurityHelper.Clean(dto.ImageUrl);
            producto.Activo = dto.IsActive;
            RepoProducto.Update(producto);
            return Mapper.Map<ProductoDTO>(producto);
        }

        public bool Eliminar(int id)
        {
            var producto = RepoProducto.Get(id);
            if (producto == null) return false;
            if (RepoDetalle.GetAll().Any(x => x.ProductoId == id))
            {
                producto.Activo = false;
                RepoProducto.Update(producto);
                return true;
            }
            RepoProducto.Delete(id);
            return true;
        }

        public async Task<ProductoDTO?> SubirImagen(int id, IFormFile file)
        {
            var producto = RepoProducto.Get(id);
            if (producto == null) return null;
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Selecciona una imagen.");
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".webp")
                throw new InvalidOperationException("Solo se permiten imagenes PNG, JPG o WEBP.");

            var webRoot = Environment.WebRootPath ?? Path.Combine(Environment.ContentRootPath, "wwwroot");
            var folder = Path.Combine(webRoot, "uploads", "products");
            Directory.CreateDirectory(folder);
            foreach (var oldFile in Directory.GetFiles(folder, $"{id}.*"))
                File.Delete(oldFile);

            var fileName = $"{id}{extension}";
            await using (var stream = File.Create(Path.Combine(folder, fileName)))
            {
                await file.CopyToAsync(stream);
            }

            producto.Imagen = $"/uploads/products/{fileName}";
            RepoProducto.Update(producto);
            return Mapper.Map<ProductoDTO>(producto);
        }
    }
}

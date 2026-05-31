using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class PedidosService
    {
        public Repository<Pedido> RepoPedido { get; set; }
        public Repository<Usuario> RepoUsuario { get; set; }
        public Repository<Producto> RepoProducto { get; set; }
        public Repository<Sucursal> RepoSucursal { get; set; }
        public IValidator<CrearPedidoDTO> CrearValidator { get; set; }
        public IValidator<EstadoPedidoDTO> EstadoValidator { get; set; }
        public IMapper Mapper { get; set; }

        public PedidosService(Repository<Pedido> repoPedido, Repository<Usuario> repoUsuario, Repository<Producto> repoProducto, Repository<Sucursal> repoSucursal, IValidator<CrearPedidoDTO> crearValidator, IValidator<EstadoPedidoDTO> estadoValidator, IMapper mapper)
        {
            RepoPedido = repoPedido;
            RepoUsuario = repoUsuario;
            RepoProducto = repoProducto;
            RepoSucursal = repoSucursal;
            CrearValidator = crearValidator;
            EstadoValidator = estadoValidator;
            Mapper = mapper;
        }

        public List<PedidoDTO> ObtenerTodos()
        {
            return RepoPedido.Query()
                .Include(x => x.Detalles)
                    .ThenInclude(x => x.Producto)
                .Include(x => x.Sucursal)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => Mapper.Map<PedidoDTO>(x))
                .ToList();
        }

        public List<PedidoDTO> ObtenerPorUsuario(int id)
        {
            return RepoPedido.Query()
                .Include(x => x.Detalles)
                    .ThenInclude(x => x.Producto)
                .Include(x => x.Sucursal)
                .Where(x => x.UsuarioId == id)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => Mapper.Map<PedidoDTO>(x))
                .ToList();
        }

        public PedidoDTO Crear(CrearPedidoDTO dto)
        {
            CrearValidator.ValidateAndThrow(dto);
            var usuario = RepoUsuario.Get(dto.UserId) ?? throw new InvalidOperationException("Usuario no encontrado.");
            var sucursal = RepoSucursal.Get(dto.BranchId) ?? throw new InvalidOperationException("Sucursal no encontrada.");
            var pedido = new Pedido
            {
                UsuarioId = usuario.Id,
                SucursalId = sucursal.Id,
                SucursalNombre = sucursal.Nombre,
                SucursalDireccion = sucursal.Direccion,
                ClienteNombre = usuario.Nombre,
                ClienteEmail = usuario.Email,
                Estado = "enviado"
            };

            foreach (var item in dto.Items)
            {
                var producto = RepoProducto.Get(item.ProductId) ?? throw new InvalidOperationException("Producto no encontrado.");
                var detalle = new DetallePedido
                {
                    ProductoId = producto.Id,
                    ProductoNombre = producto.Nombre,
                    ProductoImagen = producto.Imagen,
                    Cantidad = item.Quantity,
                    PrecioUnitario = producto.Precio,
                    Total = producto.Precio * item.Quantity
                };
                pedido.Detalles.Add(detalle);
            }

            pedido.Total = pedido.Detalles.Sum(x => x.Total);
            RepoPedido.Insert(pedido);

            var guardado = RepoPedido.Query()
                .Include(x => x.Detalles)
                    .ThenInclude(x => x.Producto)
                .Include(x => x.Sucursal)
                .First(x => x.Id == pedido.Id);
            return Mapper.Map<PedidoDTO>(guardado);
        }

        public PedidoDTO? CambiarEstado(int id, EstadoPedidoDTO dto)
        {
            EstadoValidator.ValidateAndThrow(dto);
            var pedido = RepoPedido.Query().Include(x => x.Detalles).ThenInclude(x => x.Producto).Include(x => x.Sucursal).FirstOrDefault(x => x.Id == id);
            if (pedido == null) return null;
            pedido.Estado = dto.Status ?? "enviado";
            RepoPedido.Update(pedido);
            return Mapper.Map<PedidoDTO>(pedido);
        }
    }
}

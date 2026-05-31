using API.Models.DTOs;
using API.Models.Entities;
using AutoMapper;

namespace API.Mappers
{
    public class UsuariosProfile : Profile
    {
        public UsuariosProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Nombre))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.Role, y => y.MapFrom(z => z.Role))
                .ForMember(x => x.AuthProvider, y => y.MapFrom(z => z.AuthProvider));

            CreateMap<Producto, ProductoDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Nombre))
                .ForMember(x => x.Description, y => y.MapFrom(z => z.Descripcion))
                .ForMember(x => x.Price, y => y.MapFrom(z => z.Precio))
                .ForMember(x => x.ImageUrl, y => y.MapFrom(z => z.Imagen))
                .ForMember(x => x.IsActive, y => y.MapFrom(z => z.Activo));

            CreateMap<Sucursal, SucursalDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Nombre))
                .ForMember(x => x.Address, y => y.MapFrom(z => z.Direccion))
                .ForMember(x => x.Username, y => y.MapFrom(z => z.Usuario))
                .ForMember(x => x.Latitude, y => y.MapFrom(z => z.Latitud))
                .ForMember(x => x.Longitude, y => y.MapFrom(z => z.Longitud));

            CreateMap<DetallePedido, DetallePedidoDTO>()
                .ForMember(x => x.ProductId, y => y.MapFrom(z => z.ProductoId))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.ProductoNombre))
                .ForMember(x => x.ProductImageUrl, y => y.MapFrom(z => z.Producto != null ? z.Producto.Imagen : z.ProductoImagen))
                .ForMember(x => x.Quantity, y => y.MapFrom(z => z.Cantidad))
                .ForMember(x => x.UnitPrice, y => y.MapFrom(z => z.PrecioUnitario))
                .ForMember(x => x.Subtotal, y => y.MapFrom(z => z.Total));

            CreateMap<Pedido, PedidoDTO>()
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UsuarioId))
                .ForMember(x => x.BranchId, y => y.MapFrom(z => z.SucursalId))
                .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Sucursal != null ? z.Sucursal.Nombre : z.SucursalNombre))
                .ForMember(x => x.BranchAddress, y => y.MapFrom(z => z.Sucursal != null ? z.Sucursal.Direccion : z.SucursalDireccion))
                .ForMember(x => x.CustomerName, y => y.MapFrom(z => z.ClienteNombre))
                .ForMember(x => x.CustomerEmail, y => y.MapFrom(z => z.ClienteEmail))
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Estado))
                .ForMember(x => x.DeliveryType, y => y.MapFrom(z => z.TipoEntrega))
                .ForMember(x => x.PaymentMethod, y => y.MapFrom(z => z.MetodoPago))
                .ForMember(x => x.Branch, y => y.MapFrom(z => z.Sucursal))
                .ForMember(x => x.Items, y => y.MapFrom(z => z.Detalles));
        }
    }
}

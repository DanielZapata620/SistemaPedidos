using API.Models.DTOs;
using API.Models.Entities;
using API.Repositories;

namespace API.Services
{
    public class DashboardService
    {
        public Repository<Producto> RepoProducto { get; set; }
        public Repository<Pedido> RepoPedido { get; set; }

        public DashboardService(Repository<Producto> repoProducto, Repository<Pedido> repoPedido)
        {
            RepoProducto = repoProducto;
            RepoPedido = repoPedido;
        }

        public DashboardDTO Obtener()
        {
            var pedidos = RepoPedido.GetAll().ToList();
            return new DashboardDTO
            {
                TotalProducts = RepoProducto.GetAll().Count(x => x.Activo),
                TotalOrders = pedidos.Count,
                PendingOrders = pedidos.Count(x => x.Estado == "enviado" || x.Estado == "en preparacion"),
                TotalSales = pedidos.Sum(x => x.Total)
            };
        }
    }
}

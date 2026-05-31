using PedidoApi.Models.Dtos;
using PedidoApi.Models.Entities;
using PedidoApi.Repositories;

namespace PedidoApi.Services;

public class DashboardService
{
    private readonly GenericRepository<ProductEntity> _products;
    private readonly GenericRepository<OrderEntity> _orders;

    public DashboardService(GenericRepository<ProductEntity> products, GenericRepository<OrderEntity> orders)
    {
        _products = products;
        _orders = orders;
    }

    public DashboardDto GetSummary()
    {
        var orders = _orders.GetAll();
        return new DashboardDto
        {
            TotalProducts = _products.GetAll().Count(x => x.IsActive),
            TotalOrders = orders.Count,
            PendingOrders = orders.Count(x => x.Status != "listo para recoger"),
            TotalSales = orders.Sum(x => x.Total)
        };
    }
}

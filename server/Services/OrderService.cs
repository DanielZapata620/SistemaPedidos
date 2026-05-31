using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PedidoApi.Models.Dtos;
using PedidoApi.Models.Entities;
using PedidoApi.Repositories;

namespace PedidoApi.Services;

public class OrderService
{
    private readonly GenericRepository<OrderEntity> _orders;
    private readonly GenericRepository<ProductEntity> _products;
    private readonly GenericRepository<UserEntity> _users;
    private readonly GenericRepository<BranchEntity> _branches;
    private readonly IValidator<OrderCreateDto> _validator;
    private readonly IMapper _mapper;

    public OrderService(GenericRepository<OrderEntity> orders, GenericRepository<ProductEntity> products, GenericRepository<UserEntity> users, GenericRepository<BranchEntity> branches, IValidator<OrderCreateDto> validator, IMapper mapper)
    {
        _orders = orders;
        _products = products;
        _users = users;
        _branches = branches;
        _validator = validator;
        _mapper = mapper;
    }

    public List<OrderDto> GetAll()
    {
        return _orders.Query()
            .Include(x => x.Items)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(order => _mapper.Map<OrderDto>(order))
            .ToList();
    }

    public List<OrderDto> GetByUser(int userId)
    {
        return _orders.Query()
            .Include(x => x.Items)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(order => _mapper.Map<OrderDto>(order))
            .ToList();
    }

    public OrderDto Create(OrderCreateDto dto)
    {
        _validator.ValidateAndThrow(dto);
        var user = _users.GetById(dto.UserId) ?? throw new KeyNotFoundException("Usuario no encontrado.");
        var branch = _branches.GetById(dto.BranchId) ?? throw new KeyNotFoundException("Sucursal no encontrada.");
        var order = new OrderEntity
        {
            UserId = user.Id,
            BranchId = branch.Id,
            BranchName = branch.Name,
            BranchAddress = branch.Address,
            CustomerName = user.Name,
            CustomerEmail = user.Email,
            Status = "enviado",
            DeliveryType = "recoger en tienda",
            PaymentMethod = "pago en tienda"
        };

        foreach (var item in dto.Items)
        {
            var product = _products.GetById(item.ProductId) ?? throw new KeyNotFoundException($"Producto {item.ProductId} no encontrado.");
            if (!product.IsActive)
            {
                throw new InvalidOperationException($"El producto {product.Name} no esta disponible.");
            }

            order.Items.Add(new OrderItemEntity
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = item.Quantity,
                Subtotal = product.Price * item.Quantity
            });
        }

        order.Total = order.Items.Sum(x => x.Subtotal);
        return _mapper.Map<OrderDto>(_orders.Add(order));
    }

    public OrderDto UpdateStatus(int id, string status)
    {
        if (status is not ("enviado" or "en preparacion" or "listo para recoger"))
        {
            throw new InvalidOperationException("Estado de pedido no valido.");
        }

        var order = _orders.GetById(id) ?? throw new KeyNotFoundException("Pedido no encontrado.");
        order.Status = status;
        return _mapper.Map<OrderDto>(_orders.Update(order));
    }
}

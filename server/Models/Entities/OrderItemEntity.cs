namespace PedidoApi.Models.Entities;

public class OrderItemEntity : BaseEntity
{
    public int OrderId { get; set; }
    public OrderEntity? Order { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}

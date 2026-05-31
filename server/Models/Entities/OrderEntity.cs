namespace PedidoApi.Models.Entities;

public class OrderEntity : BaseEntity
{
    public int UserId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string BranchAddress { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string Status { get; set; } = "enviado";
    public string DeliveryType { get; set; } = "recoger en tienda";
    public string PaymentMethod { get; set; } = "pago en tienda";
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderItemEntity> Items { get; set; } = new();
}

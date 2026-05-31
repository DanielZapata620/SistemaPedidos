namespace PedidoApi.Models.Dtos;

public class OrderItemCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderCreateDto
{
    public int UserId { get; set; }
    public int BranchId { get; set; }
    public List<OrderItemCreateDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string BranchAddress { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string DeliveryType { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderStatusUpdateDto
{
    public string Status { get; set; } = string.Empty;
}

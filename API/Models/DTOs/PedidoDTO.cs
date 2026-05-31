namespace API.Models.DTOs
{
    public class CrearDetallePedidoDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CrearPedidoDTO
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public List<CrearDetallePedidoDTO> Items { get; set; } = new List<CrearDetallePedidoDTO>();
    }

    public class DetallePedidoDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string ProductImageUrl { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    public class PedidoDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; } = "";
        public string BranchAddress { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string Status { get; set; } = "";
        public string DeliveryType { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public SucursalDTO? Branch { get; set; }
        public List<DetallePedidoDTO> Items { get; set; } = new List<DetallePedidoDTO>();
    }

    public class EstadoPedidoDTO
    {
        public string? Status { get; set; }
    }
}

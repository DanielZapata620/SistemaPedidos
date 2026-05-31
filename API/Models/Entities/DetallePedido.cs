namespace API.Models.Entities
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = "";
        public string ProductoImagen { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public Pedido? Pedido { get; set; }
        public Producto? Producto { get; set; }
    }
}

namespace API.Models.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? SucursalId { get; set; }
        public string SucursalNombre { get; set; } = "";
        public string SucursalDireccion { get; set; } = "";
        public string ClienteNombre { get; set; } = "";
        public string ClienteEmail { get; set; } = "";
        public string Estado { get; set; } = "enviado";
        public string TipoEntrega { get; set; } = "recoger en tienda";
        public string MetodoPago { get; set; } = "pago en tienda";
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Usuario? Usuario { get; set; }
        public Sucursal? Sucursal { get; set; }
        public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}

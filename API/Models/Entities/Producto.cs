namespace API.Models.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; }
        public string Imagen { get; set; } = "";
        public bool Activo { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
    }
}

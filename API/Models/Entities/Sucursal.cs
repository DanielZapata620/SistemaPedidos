namespace API.Models.Entities
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string ContrasenaHash { get; set; } = "";
        public decimal Latitud { get; set; } = 19.4326m;
        public decimal Longitud { get; set; } = -99.1332m;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}

namespace API.Models.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string NombreUsuario { get; set; } = "";
        public string Contrasena { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "cliente";
        public string AuthProvider { get; set; } = "local";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public ICollection<Reaccionresena> Reaccionresenas { get; set; } = new List<Reaccionresena>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}

namespace API.Models.Entities
{
    public class Resena
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombrePlatillo { get; set; } = null!;
        public int Calificacion { get; set; }
        public string UbicacionEstablecimiento { get; set; } = null!;
        public string? Telefono { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public Usuario Usuario { get; set; } = null!;
        public ICollection<Reaccionresena> Reaccionresenas { get; set; } = new List<Reaccionresena>();
    }
}

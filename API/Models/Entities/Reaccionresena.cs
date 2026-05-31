namespace API.Models.Entities
{
    public class Reaccionresena
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ResenaId { get; set; }
        public int Tipo { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public Resena Resena { get; set; } = null!;
    }
}

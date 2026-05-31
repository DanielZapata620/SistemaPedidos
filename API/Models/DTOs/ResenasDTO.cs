namespace API.Models.DTOs
{
    public class CrearResenaDTO
    {
        public string? NombrePlatillo { get; set; }
        public int Calificacion { get; set; }
        public string? UbicacionEstablecimiento { get; set; }
        public string? Telefono { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    public class EditarResenaDTO
    {
        public int Id { get; set; }
        public string? NombrePlatillo { get; set; }
        public int Calificacion { get; set; }
        public string? UbicacionEstablecimiento { get; set; }
        public string? Telefono { get; set; }
        public string? ImagenBase64 { get; set; }
    }

    public class ResenaDTO
    {
        public int Id { get; set; }
        public string NombrePlatillo { get; set; } = null!;
        public int Calificacion { get; set; }
        public string UbicacionEstablecimiento { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Imagen { get; set; } = null!;
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int MiReaccion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public UsuarioDTO Usuario { get; set; } = null!;
    }
}

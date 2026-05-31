namespace API.Models.DTOs
{
    public class SucursalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Username { get; set; } = "";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class CrearSucursalDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public decimal Latitude { get; set; } = 19.4326m;
        public decimal Longitude { get; set; } = -99.1332m;
    }

    public class EditarSucursalDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public decimal Latitude { get; set; } = 19.4326m;
        public decimal Longitude { get; set; } = -99.1332m;
    }
}

namespace API.Models.DTOs
{
    public class RegistroUsuarioDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string AuthProvider { get; set; } = "local";
    }

    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string AuthProvider { get; set; } = null!;
        public int? BranchId { get; set; }
    }
}

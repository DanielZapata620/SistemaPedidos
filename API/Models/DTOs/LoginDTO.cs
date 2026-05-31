namespace API.Models.DTOs
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public UsuarioDTO User { get; set; } = null!;
    }

    public class GoogleLoginDTO
    {
        public string? Credential { get; set; }
        public string? Role { get; set; }
    }
}

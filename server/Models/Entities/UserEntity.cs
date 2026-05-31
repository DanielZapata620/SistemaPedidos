namespace PedidoApi.Models.Entities;

public class UserEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "cliente";
    public string AuthProvider { get; set; } = "local";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

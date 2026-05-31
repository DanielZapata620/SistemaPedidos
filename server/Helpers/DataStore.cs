using PedidoApi.Models.Entities;

namespace PedidoApi.Helpers;

public class DataStore
{
    public List<UserEntity> Users { get; } = new();
    public List<ProductEntity> Products { get; } = new();
    public List<OrderEntity> Orders { get; } = new();

    public DataStore()
    {
        Users.AddRange(new[]
        {
            new UserEntity { Id = 1, Name = "Administrador", Email = "admin@cafe.com", PasswordHash = HashHelper.Sha256("Admin123!"), Role = "admin" },
            new UserEntity { Id = 2, Name = "Cliente Demo", Email = "cliente@cafe.com", PasswordHash = HashHelper.Sha256("Cliente123!"), Role = "cliente" }
        });

        Products.AddRange(new[]
        {
            new ProductEntity { Id = 1, Name = "Cafe Americano", Description = "Cafe caliente clasico.", Price = 15, ImageUrl = "/assets/img/art01.png" },
            new ProductEntity { Id = 2, Name = "Cappuccino", Description = "Espresso con leche espumada.", Price = 25, ImageUrl = "/assets/img/art02.png" },
            new ProductEntity { Id = 3, Name = "Latte Frio", Description = "Cafe con leche servido frio.", Price = 28, ImageUrl = "/assets/img/art03.png" },
            new ProductEntity { Id = 4, Name = "Chocolate Caliente", Description = "Bebida dulce con cacao.", Price = 22, ImageUrl = "/assets/img/art04.png" },
            new ProductEntity { Id = 5, Name = "Malteada de Moka", Description = "Moka frio cremoso.", Price = 20, ImageUrl = "/assets/img/art05.png" },
            new ProductEntity { Id = 6, Name = "Muffin de Arandano", Description = "Pan dulce individual.", Price = 18, ImageUrl = "/assets/img/art06.png" },
            new ProductEntity { Id = 7, Name = "Croissant", Description = "Pan hojaldrado.", Price = 15, ImageUrl = "/assets/img/art07.png" },
            new ProductEntity { Id = 8, Name = "Galleta con Chispas", Description = "Galleta de chocolate.", Price = 10, ImageUrl = "/assets/img/art08.png" },
            new ProductEntity { Id = 9, Name = "Smoothie de Fresa", Description = "Bebida fria de fruta.", Price = 25, ImageUrl = "/assets/img/art09.png" },
            new ProductEntity { Id = 10, Name = "Sandwich de Jamon y Queso", Description = "Pan con jamon y queso.", Price = 30, ImageUrl = "/assets/img/art10.png" },
            new ProductEntity { Id = 11, Name = "Frappuccino Caramel", Description = "Bebida fria con caramelo.", Price = 32, ImageUrl = "/assets/img/art11.png" },
            new ProductEntity { Id = 12, Name = "Matcha Latte", Description = "Te matcha con leche.", Price = 30, ImageUrl = "/assets/img/art12.png" },
            new ProductEntity { Id = 13, Name = "Donut Glaseada", Description = "Pan dulce glaseado.", Price = 12, ImageUrl = "/assets/img/art13.png" },
            new ProductEntity { Id = 14, Name = "Pumpkin Spice Latte", Description = "Latte especiado de temporada.", Price = 35, ImageUrl = "/assets/img/art14.png" },
            new ProductEntity { Id = 15, Name = "Bagel con Queso Crema", Description = "Bagel suave con queso.", Price = 22, ImageUrl = "/assets/img/art15.png" },
            new ProductEntity { Id = 16, Name = "Te Helado de Limon", Description = "Te frio con limon.", Price = 20, ImageUrl = "/assets/img/art16.png" }
        });
    }
}

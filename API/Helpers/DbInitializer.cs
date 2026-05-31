using API.Models.Entities;

namespace API.Helpers
{
    public class DbInitializer
    {
        public static void Seed(PlataformaalimentosContext db)
        {
            if (!db.Usuarios.Any(x => x.Role == "admin"))
            {
                db.Usuarios.Add(new Usuario
                {
                    Nombre = "Admin",
                    Email = "admin@sistemaventas.com",
                    PasswordHash = HashHelper.Sha256("123456"),
                    Role = "admin",
                    AuthProvider = "local"
                });
            }

            if (!db.Productos.Any())
            {
                var productos = new List<Producto>
                {
                    new Producto { Nombre = "Cafe Americano", Descripcion = "Cafe caliente clasico.", Precio = 15, Imagen = "/assets/img/art01.png" },
                    new Producto { Nombre = "Cappuccino", Descripcion = "Espresso con leche espumada.", Precio = 25, Imagen = "/assets/img/art02.png" },
                    new Producto { Nombre = "Latte Frio", Descripcion = "Cafe con leche servido frio.", Precio = 28, Imagen = "/assets/img/art03.png" },
                    new Producto { Nombre = "Chocolate Caliente", Descripcion = "Bebida dulce con cacao.", Precio = 22, Imagen = "/assets/img/art04.png" },
                    new Producto { Nombre = "Malteada de Moka", Descripcion = "Moka frio cremoso.", Precio = 20, Imagen = "/assets/img/art05.png" },
                    new Producto { Nombre = "Muffin de Arandano", Descripcion = "Pan dulce con arandanos.", Precio = 18, Imagen = "/assets/img/art06.png" },
                    new Producto { Nombre = "Croissant", Descripcion = "Pan hojaldrado.", Precio = 15, Imagen = "/assets/img/art07.png" },
                    new Producto { Nombre = "Galleta con Chispas", Descripcion = "Galleta dulce.", Precio = 10, Imagen = "/assets/img/art08.png" },
                    new Producto { Nombre = "Smoothie de Fresa", Descripcion = "Bebida fria de fresa.", Precio = 24, Imagen = "/assets/img/art09.png" },
                    new Producto { Nombre = "Sandwich de Jamon", Descripcion = "Sandwich preparado.", Precio = 32, Imagen = "/assets/img/art10.png" },
                    new Producto { Nombre = "Frappuccino", Descripcion = "Cafe frio cremoso.", Precio = 30, Imagen = "/assets/img/art11.png" },
                    new Producto { Nombre = "Matcha Latte", Descripcion = "Te matcha con leche.", Precio = 35, Imagen = "/assets/img/art12.png" },
                    new Producto { Nombre = "Brownie", Descripcion = "Postre de chocolate.", Precio = 18, Imagen = "/assets/img/art13.png" },
                    new Producto { Nombre = "Te Chai", Descripcion = "Infusion especiada.", Precio = 22, Imagen = "/assets/img/art14.png" },
                    new Producto { Nombre = "Bagel con Queso Crema", Descripcion = "Bagel suave con queso.", Precio = 22, Imagen = "/assets/img/art15.png" },
                    new Producto { Nombre = "Te Helado de Limon", Descripcion = "Te frio con limon.", Precio = 20, Imagen = "/assets/img/art16.png" }
                };
                db.Productos.AddRange(productos);
            }

            db.SaveChanges();
        }
    }
}

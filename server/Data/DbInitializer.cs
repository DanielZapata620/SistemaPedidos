using PedidoApi.Helpers;
using PedidoApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace PedidoApi.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        EnsureSchema(context);

        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new UserEntity { Name = "Administrador", Email = "admin@cafe.com", PasswordHash = HashHelper.Sha256("Admin123!"), Role = "admin" },
                new UserEntity { Name = "Cliente Demo", Email = "cliente@cafe.com", PasswordHash = HashHelper.Sha256("Cliente123!"), Role = "cliente" }
            );
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new ProductEntity { Name = "Cafe Americano", Description = "Cafe caliente clasico.", Price = 15, ImageUrl = "/assets/img/art01.png" },
                new ProductEntity { Name = "Cappuccino", Description = "Espresso con leche espumada.", Price = 25, ImageUrl = "/assets/img/art02.png" },
                new ProductEntity { Name = "Latte Frio", Description = "Cafe con leche servido frio.", Price = 28, ImageUrl = "/assets/img/art03.png" },
                new ProductEntity { Name = "Chocolate Caliente", Description = "Bebida dulce con cacao.", Price = 22, ImageUrl = "/assets/img/art04.png" },
                new ProductEntity { Name = "Malteada de Moka", Description = "Moka frio cremoso.", Price = 20, ImageUrl = "/assets/img/art05.png" },
                new ProductEntity { Name = "Muffin de Arandano", Description = "Pan dulce individual.", Price = 18, ImageUrl = "/assets/img/art06.png" },
                new ProductEntity { Name = "Croissant", Description = "Pan hojaldrado.", Price = 15, ImageUrl = "/assets/img/art07.png" },
                new ProductEntity { Name = "Galleta con Chispas", Description = "Galleta de chocolate.", Price = 10, ImageUrl = "/assets/img/art08.png" },
                new ProductEntity { Name = "Smoothie de Fresa", Description = "Bebida fria de fruta.", Price = 25, ImageUrl = "/assets/img/art09.png" },
                new ProductEntity { Name = "Sandwich de Jamon y Queso", Description = "Pan con jamon y queso.", Price = 30, ImageUrl = "/assets/img/art10.png" },
                new ProductEntity { Name = "Frappuccino Caramel", Description = "Bebida fria con caramelo.", Price = 32, ImageUrl = "/assets/img/art11.png" },
                new ProductEntity { Name = "Matcha Latte", Description = "Te matcha con leche.", Price = 30, ImageUrl = "/assets/img/art12.png" },
                new ProductEntity { Name = "Donut Glaseada", Description = "Pan dulce glaseado.", Price = 12, ImageUrl = "/assets/img/art13.png" },
                new ProductEntity { Name = "Pumpkin Spice Latte", Description = "Latte especiado de temporada.", Price = 35, ImageUrl = "/assets/img/art14.png" },
                new ProductEntity { Name = "Bagel con Queso Crema", Description = "Bagel suave con queso.", Price = 22, ImageUrl = "/assets/img/art15.png" },
                new ProductEntity { Name = "Te Helado de Limon", Description = "Te frio con limon.", Price = 20, ImageUrl = "/assets/img/art16.png" }
            );
        }

        if (!context.Branches.Any())
        {
            context.Branches.Add(new BranchEntity
            {
                Name = "Sucursal Centro",
                Address = "Centro, CDMX",
                Latitude = 19.4326,
                Longitude = -99.1332,
                Username = "centro",
                PasswordHash = HashHelper.Sha256("Centro123!")
            });
        }

        context.SaveChanges();
    }

    private static void EnsureSchema(AppDbContext context)
    {
        context.Database.ExecuteSqlRaw("""
            CREATE TABLE IF NOT EXISTS `Branches` (
                `Id` int NOT NULL AUTO_INCREMENT,
                `Name` varchar(100) NOT NULL,
                `Address` varchar(250) NOT NULL,
                `Latitude` double NOT NULL,
                `Longitude` double NOT NULL,
                `Username` varchar(80) NOT NULL,
                `PasswordHash` longtext NOT NULL,
                `CreatedAt` datetime(6) NOT NULL,
                CONSTRAINT `PK_Branches` PRIMARY KEY (`Id`)
            );
            """);

        AddColumnIfMissing(context, "Orders", "BranchId", "int NOT NULL DEFAULT 1");
        AddColumnIfMissing(context, "Orders", "BranchName", "varchar(100) NOT NULL DEFAULT ''");
        AddColumnIfMissing(context, "Orders", "BranchAddress", "varchar(250) NOT NULL DEFAULT ''");
    }

    private static void AddColumnIfMissing(AppDbContext context, string table, string column, string definition)
    {
        var connection = context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        using var check = connection.CreateCommand();
        check.CommandText = """
            SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column
            """;
        var tableParam = check.CreateParameter();
        tableParam.ParameterName = "@table";
        tableParam.Value = table;
        check.Parameters.Add(tableParam);
        var columnParam = check.CreateParameter();
        columnParam.ParameterName = "@column";
        columnParam.Value = column;
        check.Parameters.Add(columnParam);

        var exists = Convert.ToInt32(check.ExecuteScalar()) > 0;
        if (exists) return;

        using var alter = connection.CreateCommand();
        alter.CommandText = $"ALTER TABLE `{table}` ADD COLUMN `{column}` {definition}";
        alter.ExecuteNonQuery();
    }
}

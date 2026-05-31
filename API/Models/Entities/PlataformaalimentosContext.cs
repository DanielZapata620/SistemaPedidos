using Microsoft.EntityFrameworkCore;

namespace API.Models.Entities
{
    public class PlataformaalimentosContext : DbContext
    {
        public PlataformaalimentosContext(DbContextOptions<PlataformaalimentosContext> options) : base(options)
        {
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Resena> Resenas { get; set; }
        public virtual DbSet<Reaccionresena> Reaccionresenas { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<DetallePedido> DetallesPedido { get; set; }
        public virtual DbSet<Sucursal> Sucursales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasColumnName("Name").HasMaxLength(120);
                entity.Property(e => e.Email).HasMaxLength(180);
                entity.Property(e => e.PasswordHash).HasMaxLength(120);
                entity.Property(e => e.Role).HasMaxLength(30);
                entity.Property(e => e.AuthProvider).HasMaxLength(30);
                entity.Property(e => e.CreatedAt);
                entity.Ignore(e => e.Apellido);
                entity.Ignore(e => e.NombreUsuario);
                entity.Ignore(e => e.Contrasena);
            });

            modelBuilder.Entity<Resena>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombrePlatillo).HasMaxLength(100);
                entity.Property(e => e.UbicacionEstablecimiento).HasMaxLength(200);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.HasOne(e => e.Usuario)
                    .WithMany(e => e.Resenas)
                    .HasForeignKey(e => e.UsuarioId);
            });

            modelBuilder.Entity<Reaccionresena>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UsuarioId, e.ResenaId }).IsUnique();
                entity.HasOne(e => e.Usuario)
                    .WithMany(e => e.Reaccionresenas)
                    .HasForeignKey(e => e.UsuarioId);
                entity.HasOne(e => e.Resena)
                    .WithMany(e => e.Reaccionresenas)
                    .HasForeignKey(e => e.ResenaId);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasColumnName("Name").HasMaxLength(120);
                entity.Property(e => e.Descripcion).HasColumnName("Description").HasMaxLength(400);
                entity.Property(e => e.Precio).HasColumnName("Price").HasPrecision(10, 2);
                entity.Property(e => e.Imagen).HasColumnName("ImageUrl").HasMaxLength(400);
                entity.Property(e => e.Activo).HasColumnName("IsActive");
            });

            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.ToTable("Branches");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasColumnName("Name").HasMaxLength(120);
                entity.Property(e => e.Direccion).HasColumnName("Address").HasMaxLength(250);
                entity.Property(e => e.Usuario).HasColumnName("Username").HasMaxLength(80);
                entity.Property(e => e.ContrasenaHash).HasColumnName("PasswordHash").HasMaxLength(120);
                entity.Property(e => e.Latitud).HasColumnName("Latitude").HasPrecision(10, 6);
                entity.Property(e => e.Longitud).HasColumnName("Longitude").HasPrecision(10, 6);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UsuarioId).HasColumnName("UserId");
                entity.Property(e => e.SucursalId).HasColumnName("BranchId");
                entity.Property(e => e.SucursalNombre).HasColumnName("BranchName").HasMaxLength(100);
                entity.Property(e => e.SucursalDireccion).HasColumnName("BranchAddress").HasMaxLength(250);
                entity.Property(e => e.ClienteNombre).HasColumnName("CustomerName").HasMaxLength(120);
                entity.Property(e => e.ClienteEmail).HasColumnName("CustomerEmail").HasMaxLength(180);
                entity.Property(e => e.Estado).HasColumnName("Status").HasMaxLength(40);
                entity.Property(e => e.TipoEntrega).HasColumnName("DeliveryType").HasMaxLength(60);
                entity.Property(e => e.MetodoPago).HasColumnName("PaymentMethod").HasMaxLength(60);
                entity.Property(e => e.Total).HasPrecision(10, 2);
                entity.HasOne(e => e.Usuario).WithMany(e => e.Pedidos).HasForeignKey(e => e.UsuarioId);
                entity.HasOne(e => e.Sucursal).WithMany(e => e.Pedidos).HasForeignKey(e => e.SucursalId);
            });

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PedidoId).HasColumnName("OrderId");
                entity.Property(e => e.ProductoId).HasColumnName("ProductId");
                entity.Property(e => e.ProductoNombre).HasColumnName("ProductName").HasMaxLength(120);
                entity.Ignore(e => e.ProductoImagen);
                entity.Property(e => e.Cantidad).HasColumnName("Quantity");
                entity.Property(e => e.PrecioUnitario).HasColumnName("UnitPrice").HasPrecision(10, 2);
                entity.Property(e => e.Total).HasColumnName("Subtotal").HasPrecision(10, 2);
                entity.HasOne(e => e.Pedido).WithMany(e => e.Detalles).HasForeignKey(e => e.PedidoId);
                entity.HasOne(e => e.Producto).WithMany(e => e.DetallesPedido).HasForeignKey(e => e.ProductoId);
            });
        }
    }
}

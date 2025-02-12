using Microsoft.EntityFrameworkCore;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Entrega> Entregas { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relacionamentos
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Pedidos)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Entregas)
            .WithOne(e => e.Usuario)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pedido>()
            .HasIndex(p => p.NumeroPedido)
            .IsUnique();

        modelBuilder.Entity<Pedido>()
            .Property(p => p.StatusPedido)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Entrega>()
            .HasIndex(e => e.PedidoId)
            .IsUnique();

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Entrega)
            .WithOne(e => e.Pedido)
            .HasForeignKey<Entrega>(e => e.PedidoId);

        modelBuilder.Entity<Entrega>()
            .Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Notification>()
            .Property(n => n.Tipo)
            .HasConversion<string>();
    }
}

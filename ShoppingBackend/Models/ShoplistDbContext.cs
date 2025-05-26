using Microsoft.EntityFrameworkCore;
using ShoppingBackend.Models;

public partial class ShoplistDbContext : DbContext
{
    public ShoplistDbContext()
    {
    }

    public ShoplistDbContext(DbContextOptions<ShoplistDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Shoplist> Shoplist { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure SQL Server if no other provider has been set (e.g. in tests)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=tcp:juhasrvnorthwind.database.windows.net,1433;" +
                "Initial Catalog=ShoplistDB;Persist Security Info=False;" +
                "User ID=adminlarah;Password=kwcvSBfA8y5ixWA;" +
                "MultipleActiveResultSets=False;Encrypt=True;" +
                "TrustServerCertificate=False;Connection Timeout=30;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shoplist>(entity =>
        {
            entity.ToTable("Shoplist");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Item)
                  .HasMaxLength(50)
                  .IsFixedLength()
                  .HasColumnName("item");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

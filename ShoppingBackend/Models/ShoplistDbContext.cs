using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoppingBackend.Models;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-IQ1GGQ8\\SQLEXJUHAHO; Database=ShoplistDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

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

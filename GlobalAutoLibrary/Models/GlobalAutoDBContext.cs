using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GlobalAutoLibrary.Models;

public partial class GlobalAutoDBContext : DbContext
{
    public GlobalAutoDBContext(DbContextOptions<GlobalAutoDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F05EFF80931E");

            entity.HasIndex(e => e.BrandName, "UQ__Brands__2206CE9B9665709C").IsUnique();

            entity.Property(e => e.BrandName).HasMaxLength(50);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Cars__68A0342EC0AA5EF5");

            entity.HasIndex(e => e.Vin, "UQ__Cars__C5DF234C3E0AE540").IsUnique();

            entity.Property(e => e.Color).HasMaxLength(30);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VIN");

            entity.HasOne(d => d.Brand).WithMany(p => p.Cars).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.Seller).WithMany(p => p.Cars)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB4FA1E6A");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E43560FDB6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534171D8FBA").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserRole).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

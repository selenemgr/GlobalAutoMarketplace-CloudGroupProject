using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GlobalAutoLibrary.Models;

public partial class GlobalAutoDBContext : DbContext
{
    public GlobalAutoDBContext()
    {
    }

    public GlobalAutoDBContext(DbContextOptions<GlobalAutoDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GlobalAutoDB;Trusted_Connection=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F05EE0D274F8");

            entity.HasIndex(e => e.Bname, "UQ__Brands__3FF350828D985497").IsUnique();

            entity.Property(e => e.Bname)
                .HasMaxLength(50)
                .HasColumnName("BName");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Cars__68A0342EF7CB5787");

            entity.HasIndex(e => e.Vin, "UQ__Cars__C5DF234CDFA96D4A").IsUnique();

            entity.Property(e => e.Color).HasMaxLength(30);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VIN");

            entity.HasOne(d => d.Brand).WithMany(p => p.Cars).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.VehicleTypeId)
                .HasConstraintName("FK_Cars_VehicleTypes_TypeId");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.VehicleTypeId).HasName("PK__VehicleT__9F44964343B1F9AD");

            entity.HasIndex(e => e.TypeName, "UQ__VehicleT__D4E7DFA8AF742F71").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

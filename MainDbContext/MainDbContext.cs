using Core.Models;
using DbCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DbCore
{
    public class MainDbContext:DbContext
    {
        public MainDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(CoreSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pricelist>(PricelistsConfigure);
        }

        protected void PricelistsConfigure(EntityTypeBuilder<Pricelist> builder)
        {
            builder.ToTable("pricelists");
            
            builder.HasKey(p => p.Id)
                .HasName("PRIMARY");



            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("char(36)");

            builder.Property(p => p.SupplierName)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Name)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.LastUpdate)
                .IsRequired(false)
                .HasColumnType("timestamp");

            builder.Property(p => p.UncheckedCount)
                .IsRequired()
                .HasColumnType("int")
                .HasDefaultValue(0);

            builder.Property(p => p.MinStockAvail)
                .IsRequired()
                .HasColumnType("int")
                .HasDefaultValue(3);

            builder.Property(p => p.PreorderInDays)
                .IsRequired()
                .HasColumnType("int")
                .HasDefaultValue(2);

            builder.Property(p => p.IsFavorite)
                .IsRequired()
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(false);

            builder.Property(p => p.Controller)
                .IsRequired()
                .HasColumnType("varchar(200)");

        }

        public virtual DbSet<Pricelist> Pricelists { get; set; }
    }
}

using DbCore.Models;
using DbCore.PLModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DbCore
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramUser>(TelegramUsersConfigure);

            modelBuilder.Entity<Pricelist>(PricelistsConfigure);
            modelBuilder.Entity<VectorOffer>(VecorOffersConfigure);

            //Pricelists
            modelBuilder.Entity<DynatoneOffer>(DynatoneConfigure);
            modelBuilder.Entity<GrandmOffer>(GrandmConfigure);
            modelBuilder.Entity<PleerOffer>(PleerConfigure);
        }

        protected void TelegramUsersConfigure(EntityTypeBuilder<TelegramUser> builder)
        {
            builder.ToTable("telegramusers");

            builder.HasKey(p => p.Id)
                   .HasName("PRIMARY");



            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("char(36)");

            builder.Property(p => p.TelegramUserId)
                .IsRequired()
                .HasColumnType("bigint(255)");

            builder.Property(p => p.Role)
                .IsRequired()
                .HasColumnType("varchar(100)");
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

            builder.Property(p => p.LastPull)
                .IsRequired(false)
                .HasColumnType("timestamp");

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

            builder.Property(p => p.ExchangeRate)
                .HasColumnType("float(6,2)");

            builder.Property(p => p.Controller)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.PullMethodType)
                .IsRequired()
                .HasColumnType("int");
        }

        protected void VecorOffersConfigure(EntityTypeBuilder<VectorOffer> builder)
        {
            builder.ToTable("offers");

            builder.HasKey(o => o.Id)
                .HasName("PRIMARY");



            builder.Property(o => o.Id)
                .IsRequired()
                .HasColumnType("char(36)");

            builder.Property(o => o.IsVerified)
                .IsRequired()
                .HasColumnName("IsVerified")
                .HasColumnType("tinyint(1)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasColumnName("Status")
                .HasColumnType("int")
                .HasDefaultValue(VectorOfferStatus.Active);

            builder.Property(o => o.Sku)
                .HasColumnName("Sku")
                .HasColumnType("varchar(150)");

            builder.Property(o => o.Brand)
                .HasColumnName("Brand")
                .HasColumnType("varchar(300)");

            builder.Property(o => o.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar(1000)");

            builder.Property(o => o.Price)
                .HasColumnName("Price")
                .HasColumnType("float(12,2)");

            builder.Property(o => o.PriceLimit)
                .HasColumnName("PriceLimit")
                .HasColumnType("float(12,2)");

            builder.Property(o => o.Pp1)
                .HasColumnName("Pp1")
                .HasColumnType("tinyint(1)");

            builder.Property(o => o.Pp2)
                .HasColumnName("Pp2")
                .HasColumnType("tinyint(1)");

            builder.Property(o => o.Mskdnt)
                .HasColumnName("Mskdnt")
                .HasColumnType("tinyint(1)");

            builder.Property(o => o.Nnach)
                .HasColumnName("Nnach")
                .HasColumnType("tinyint(1)");

            builder.Property(o => o.PreorderInDays)
                .HasColumnName("PreorderInDays")
                .HasColumnType("int");

            builder.Property(o => o.IsFavorite)
                .HasColumnName("IsFavorite")
                .HasColumnType("tinyint(1)");

            builder.Property(o => o.Supplier)
                .HasColumnName("Supplier")
                .HasColumnType("varchar(300)");

            builder.Property(o => o.PricelistId)
                .IsRequired()
                .HasColumnName("PricelistId")
                .HasColumnType("char(36)");
        }

        protected void DynatoneConfigure(EntityTypeBuilder<DynatoneOffer> builder)
        {
            builder.ToTable("dynatone");

            builder.HasKey(p => p.Id)
                .HasName("PRIMARY");



            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("char(36)");

            builder.Property(p => p.Kod)
                .HasColumnType("int");

            builder.Property(p => p.KodGruppy)
                .HasColumnType("int");

            builder.Property(p => p.Naimenovanie)
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.RRC)
                .HasColumnType("int");

            builder.Property(p => p.CenaDiler)
                .HasColumnType("int");

            builder.Property(p => p.KolichestvoDlyaOpta)
                .HasColumnType("int");

            builder.Property(p => p.VidNomenklatury)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.Brand)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.ModelSModifikaciyey)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.Articul)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Barcode)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Modifikaciya)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.KodSPrefixom)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.StranaProishojdenia)
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Ves)
                .HasColumnType("float(6,3)");

            builder.Property(p => p.Dlina)
                .HasColumnType("float(6,3)");

            builder.Property(p => p.Shirina)
                .HasColumnType("float(6,3)");

            builder.Property(p => p.Vysota)
                .HasColumnType("float(6,3)");

            builder.Property(p => p.Izobrazhenie)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.Opisanie)
                .HasColumnType("text");
        }

        protected void GrandmConfigure(EntityTypeBuilder<GrandmOffer> builder)
        {
            builder.ToTable("grandm");

            builder.HasKey(p => p.Id)
                   .HasName("PRIMARY");


            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("char(36)");

            builder.Property(p => p.Brand)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.CategoryName)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.CenaDiler)
                .HasColumnType("int");

            builder.Property(p => p.Kornevaya)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Podkategoriya1)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Podkategoriya2)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Podkategoriya3)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Podkategoriya4)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.IdTovara)
                .HasColumnType("int");

            builder.Property(p => p.NazvanieTovara)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.URL)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.KratkoeOpisanie)
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Izobrazheniya)
                .HasColumnType("varchar(500)");

            builder.Property(p => p.SvoistvoRazmer)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.SvoistvoCvet)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Articul)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.CenaProdazhi)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Ostatok)
                .HasColumnType("int");

            builder.Property(p => p.Ves)
                .HasColumnType("varchar(255)");

        }

        protected void PleerConfigure(EntityTypeBuilder<PleerOffer> builder)
        {
            builder.ToTable("pleer");

            builder.HasKey(p => p.Id)
                   .HasName("PRIMARY");


            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("char(36)");

            builder.Property(p => p.NomerTovara)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Catalog)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.KodTovara)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Naimenovanie)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Garantiya)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.Nalichie)
                .HasColumnType("int");

            builder.Property(p => p.Diler1)
                .HasColumnType("int");

            builder.Property(p => p.Diler2)
                .HasColumnType("int");

            builder.Property(p => p.Diler3)
                .HasColumnType("int");

            builder.Property(p => p.Diler4)
                .HasColumnType("int");
        }


        public virtual DbSet<TelegramUser> TelegramUsers { get; set; }

        public virtual DbSet<Pricelist> Pricelists { get; set; }
        public virtual DbSet<VectorOffer> VectorOffers { get; set; }

        public virtual DbSet<DynatoneOffer> Dynatone { get; set; }
        public virtual DbSet<GrandmOffer> Grandm { get; set; }
        public virtual DbSet<PleerOffer> Pleer { get; set; }
    }
}

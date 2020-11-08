﻿// <auto-generated />
using System;
using DbCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DbCore.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DbCore.Models.Pricelist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Controller")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<float?>("ExchangeRate")
                        .HasColumnType("float(6,2)");

                    b.Property<bool>("IsFavorite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int>("ItemsToVerify")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("LastPull")
                        .HasColumnType("timestamp");

                    b.Property<int>("MinStockAvail")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(3);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)");

                    b.Property<int>("PreorderInDays")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("pricelists");
                });

            modelBuilder.Entity("DbCore.Models.VectorOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Brand")
                        .HasColumnName("Brand")
                        .HasColumnType("varchar(300)");

                    b.Property<bool?>("IsFavorite")
                        .HasColumnName("IsFavorite")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsVerified")
                        .HasColumnName("IsVerified")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("Mskdnt")
                        .HasColumnName("Mskdnt")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("varchar(1000)");

                    b.Property<bool?>("Nnach")
                        .HasColumnName("Nnach")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("Pp1")
                        .HasColumnName("Pp1")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("Pp2")
                        .HasColumnName("Pp2")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("PreorderInDays")
                        .HasColumnName("PreorderInDays")
                        .HasColumnType("int");

                    b.Property<float?>("Price")
                        .HasColumnName("Price")
                        .HasColumnType("float(12,2)");

                    b.Property<float?>("PriceLimit")
                        .HasColumnName("PriceLimit")
                        .HasColumnType("float(12,2)");

                    b.Property<string>("Sku")
                        .HasColumnName("Sku")
                        .HasColumnType("varchar(150)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Status")
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Supplier")
                        .HasColumnName("Supplier")
                        .HasColumnType("varchar(300)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("offers");
                });

            modelBuilder.Entity("DbCore.PLModels.DynatoneOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Articul")
                        .HasColumnType("varchar(150)");

                    b.Property<long?>("Barcode")
                        .HasColumnType("bigint");

                    b.Property<string>("Brand")
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("CenaDiler")
                        .HasColumnType("int");

                    b.Property<float?>("Dlina")
                        .HasColumnType("float(6,3)");

                    b.Property<int?>("Kod")
                        .HasColumnType("int");

                    b.Property<int?>("KodGruppy")
                        .HasColumnType("int");

                    b.Property<string>("KodSPrefixom")
                        .HasColumnType("varchar(150)");

                    b.Property<int?>("KolichestvoDlyaOpta")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ModelSModifikaciyey")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Modifikaciya")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Naimenovanie")
                        .HasColumnType("varchar(1000)");

                    b.Property<int?>("RRC")
                        .HasColumnType("int");

                    b.Property<float?>("Shirina")
                        .HasColumnType("float(6,3)");

                    b.Property<string>("StranaProishojdenia")
                        .HasColumnType("varchar(150)");

                    b.Property<float?>("Ves")
                        .HasColumnType("float(6,3)");

                    b.Property<string>("VidNomenklatury")
                        .HasColumnType("varchar(500)");

                    b.Property<float?>("Vysota")
                        .HasColumnType("float(6,3)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("dynatone");
                });
#pragma warning restore 612, 618
        }
    }
}

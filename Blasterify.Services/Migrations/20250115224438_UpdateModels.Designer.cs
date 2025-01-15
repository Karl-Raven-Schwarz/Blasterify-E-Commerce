﻿// <auto-generated />
using System;
using Blasterify.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blasterify.Services.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250115224438_UpdateModels")]
    partial class UpdateModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Blasterify.Services.Models.AdministratorUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastConnectionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique();

                    b.ToTable("AdministratorUsers");
                });

            modelBuilder.Entity("Blasterify.Services.Models.ClientUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastConnectionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("MerchantOrderId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("YunoId")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("MerchantOrderId")
                        .IsUnique();

                    b.HasIndex("YunoId")
                        .IsUnique()
                        .HasFilter("[YunoId] IS NOT NULL");

                    b.ToTable("ClientUsers");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Duration")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("FirebasePosterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFree")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("PremiereDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("decimal(4,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(3,2)");

                    b.Property<decimal>("RentalPrice")
                        .HasColumnType("decimal(4,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FirebasePosterId")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Blasterify.Services.Models.MovieGenre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MovieId", "GenreId")
                        .IsUnique();

                    b.ToTable("MovieGenres");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("CheckoutSession")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("ClientUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CheckoutSession")
                        .IsUnique()
                        .HasFilter("[CheckoutSession] IS NOT NULL");

                    b.HasIndex("ClientUserId");

                    b.HasIndex("CountryId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Blasterify.Services.Models.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(4,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("MovieId", "OrderId")
                        .IsUnique();

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ClientUserId", "MovieId")
                        .IsUnique();

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Rental", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ConcurrencyToken")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ClientUserId", "MovieId", "OrderId")
                        .IsUnique();

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("Blasterify.Services.Models.PurchaseItem", b =>
                {
                    b.HasBaseType("Blasterify.Services.Models.OrderItem");

                    b.ToTable("PurchaseItems", (string)null);
                });

            modelBuilder.Entity("Blasterify.Services.Models.RentalItem", b =>
                {
                    b.HasBaseType("Blasterify.Services.Models.OrderItem");

                    b.ToTable("RentalItems", (string)null);
                });

            modelBuilder.Entity("Blasterify.Services.Models.AdministratorUser", b =>
                {
                    b.HasOne("Blasterify.Services.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Blasterify.Services.Models.ClientUser", b =>
                {
                    b.HasOne("Blasterify.Services.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Blasterify.Services.Models.MovieGenre", b =>
                {
                    b.HasOne("Blasterify.Services.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Order", b =>
                {
                    b.HasOne("Blasterify.Services.Models.ClientUser", "ClientUser")
                        .WithMany()
                        .HasForeignKey("ClientUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ClientUser");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Blasterify.Services.Models.OrderItem", b =>
                {
                    b.HasOne("Blasterify.Services.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Purchase", b =>
                {
                    b.HasOne("Blasterify.Services.Models.ClientUser", "ClientUser")
                        .WithMany()
                        .HasForeignKey("ClientUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientUser");

                    b.Navigation("Movie");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Blasterify.Services.Models.Rental", b =>
                {
                    b.HasOne("Blasterify.Services.Models.ClientUser", "ClientUser")
                        .WithMany()
                        .HasForeignKey("ClientUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blasterify.Services.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientUser");

                    b.Navigation("Movie");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Blasterify.Services.Models.PurchaseItem", b =>
                {
                    b.HasOne("Blasterify.Services.Models.OrderItem", null)
                        .WithOne()
                        .HasForeignKey("Blasterify.Services.Models.PurchaseItem", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Blasterify.Services.Models.RentalItem", b =>
                {
                    b.HasOne("Blasterify.Services.Models.OrderItem", null)
                        .WithOne()
                        .HasForeignKey("Blasterify.Services.Models.RentalItem", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
using Blasterify.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Blasterify.Services.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Country>? Countries { get; set; }
        public DbSet<AdministratorUser>? AdministratorUsers { get; set; }
        public DbSet<ClientUser>? ClientUsers { get; set; }
        public DbSet<Genre>? Genres { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<MovieGenre>? MovieGenres { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<PurchaseItem>? PurchaseItems { get; set; }
        public DbSet<RentalItem>? RentalItems { get; set; }
        public DbSet<Purchase>? Purchases { get; set; }
        public DbSet<Rental>? Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Country
            modelBuilder.Entity<Country>(builder =>
            {
                builder.HasIndex(c => c.Name).IsUnique();
                builder.HasIndex(c => c.Code).IsUnique();
            });

            // Administrator User
            modelBuilder.Entity<AdministratorUser>(builder =>
            {
                builder.HasIndex(au => au.Email).IsUnique();
                builder.HasIndex(au => new
                {
                    au.FirstName,
                    au.LastName,
                }).IsUnique();
            });

            // Client User
            modelBuilder.Entity<ClientUser>(builder =>
            {
                builder.HasIndex(cu => cu.Email).IsUnique();
                builder.HasIndex(cu => cu.YunoId).IsUnique();
                builder.HasIndex(cu => cu.MerchantOrderId).IsUnique();
            });

            // Genre
            modelBuilder.Entity<Genre>(builder =>
            {
                builder.HasIndex(g => g.Name).IsUnique();
            });
            
            // Movie
            modelBuilder.Entity<Movie>(builder =>
            {
                builder.HasIndex(m => m.Name).IsUnique();
                builder.HasIndex(m => m.FirebasePosterId).IsUnique();
            });

            // Movie Genre
            modelBuilder.Entity<MovieGenre>(builder =>
            {
                builder.HasIndex(mg => new
                {
                    mg.MovieId,
                    mg.GenreId,
                }).IsUnique();
            });

            // Order
            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasIndex(o => o.CheckoutSession).IsUnique();
                builder.HasOne(o => o.ClientUser)
                       .WithMany()
                       .HasForeignKey(o => o.ClientUserId)
                       .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(o => o.Country)
                       .WithMany()
                       .HasForeignKey(o => o.CountryId)
                       .OnDelete(DeleteBehavior.NoAction);
            });

            // Order Item
            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.HasIndex(pi => new
                {
                    pi.MovieId,
                    pi.OrderId,
                }).IsUnique();
            });

            // Purchase Item
            modelBuilder.Entity<PurchaseItem>(builder =>
            {
                builder.ToTable("PurchaseItems");
            });

            // Rental Item
            modelBuilder.Entity<RentalItem>(builder =>
            {
                builder.ToTable("RentalItems");
            });

            // Purchase
            modelBuilder.Entity<Purchase>(builder =>
            {
                builder.HasIndex(p => new
                {
                    p.ClientUserId,
                    p.MovieId,
                }).IsUnique();
            });

            // Rental
            modelBuilder.Entity<Rental>(builder =>
            {
                builder.HasIndex(r => new
                {
                    r.ClientUserId,
                    r.MovieId,
                    r.OrderId
                }).IsUnique();
            });
        }
    }
}
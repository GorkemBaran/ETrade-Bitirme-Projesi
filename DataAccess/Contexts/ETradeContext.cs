using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class ETradeContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductStore> ProductStores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public ETradeContext(DbContextOptions options) : base(options)
        {
             
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductStore>().HasKey(ps => new { ps.ProductId, ps.StoreId });

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductStore>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStores)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductStore>()
                //.ToTable("Urunler")
                .HasOne(ps => ps.Store)
                .WithMany(p => p.ProductStores)
                .HasForeignKey(ps => ps.StoreId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasOne(ud => ud.User)
                .WithOne(u => u.UserDetail)
                .HasForeignKey<UserDetail>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasOne(ud => ud.Country)
                .WithMany(c => c.UserDetails)
                .HasForeignKey(ud => ud.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
                .HasOne(ud => ud.City)
                .WithMany(c => c.UserDetails)
                .HasForeignKey(ud => ud.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>()
                .HasOne(ci => ci.Country)
                .WithMany(co => co.Cities)
                .HasForeignKey(ci => ci.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName).IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);
        }
    }
}

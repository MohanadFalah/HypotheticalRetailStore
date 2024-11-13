using HypotheticalRetailStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HypotheticalRetailStore.DATA;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> UsersTable { get; set; }
    public DbSet<Product> ProductsTable { get; set; }
    public DbSet<Supplier> SuppliersTable { get; set; }
    public DbSet<StockTransaction> StockTransactionsTable { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete products when a supplier is deleted

        modelBuilder.Entity<StockTransaction>()
            .HasOne(st => st.Product)
            .WithMany()
            .HasForeignKey(st => st.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of product if related to a transaction
        
        //add defualt user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = "Admin"
            });
    }
}

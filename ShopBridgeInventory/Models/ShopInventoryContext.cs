namespace ShopBridgeInventory.Models
{
    using Microsoft.EntityFrameworkCore;

    public class ShopInventoryContext : DbContext
    {
        public ShopInventoryContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Android TV",
                Description = "Full HD LED Smart Android TV",
                Category = "Electronics",
                AvailableQuantity = 11,
                Color = "Black",
                Price = 12000,
            }, new Product
            {
                ProductId = 2,
                Name = "Mobile",
                Description = "HD Pin Hole Display, 16 MP Quad Rear Camera",
                Category = "Electronics",
                AvailableQuantity = 5,
                Color = "White",
                Price = 10000,
            });
        }
    }

}

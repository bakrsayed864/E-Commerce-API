using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Own_Service.Models
{
    public class CommerceDbContext:IdentityDbContext<ApplicationUser>
    {
        public CommerceDbContext()
        {}
        public CommerceDbContext(DbContextOptions<CommerceDbContext> _options) : base(_options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ProductReview> productsReviews { get; set; }
        public DbSet<UnConfirmedOrder> UnConfirmedOrders { get; set; }

    }
}

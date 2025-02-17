using Microsoft.EntityFrameworkCore;
using Bitik.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Bitik.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
       
        public DbSet<AppUser> User { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductViewModel> productview { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductViewModel>().HasNoKey().ToView("productview");
            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)  
                .WithMany(u => u.Orders)  
                .HasForeignKey(o => o.UserId);  

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        
       

    }
}

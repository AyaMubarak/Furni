using Landing.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Landing.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Service>Services { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Testimonial>Testimonials { get; set; }
        public DbSet<Blog>Blogs { get; set; }
        public DbSet<Team>Teams { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using One_more_sorting_center.Models;

namespace One_more_sorting_center.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.ProblemType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedDate)
                .HasDefaultValueSql("now() at time zone 'utc'");
        }

    }
}
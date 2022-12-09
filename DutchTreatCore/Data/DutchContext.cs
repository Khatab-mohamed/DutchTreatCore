using DutchTreatCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreatCore.Data
{
    public class DutchContext : DbContext
    {
        public DutchContext(DbContextOptions<DutchContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Categories> Categories { get; set; }
        // public DbSet<Transcation> Transactions { get; set; }  // fixed naming
        public DbSet<Transaction> Transactions { get; set; }
    }
}

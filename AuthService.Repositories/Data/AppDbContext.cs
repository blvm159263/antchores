using Microsoft.EntityFrameworkCore;
using AuthService.Repositories.Entities;

namespace AuthService.Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Tasker> Taskers { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Account>()
                .HasOne(p => p.Customer)
                .WithOne(p => p.Account)
                .HasForeignKey<Customer>(p => p.AccountId);

            modelBuilder
                .Entity<Account>()
                .HasOne(account => account.Tasker)
                .WithOne(tasker => tasker.Account)
                .HasForeignKey<Tasker>(tasker => tasker.AccountId);

        }
    }
}
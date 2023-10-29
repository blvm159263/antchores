using Microsoft.EntityFrameworkCore;
using AuthService.Repositories.Entities;

namespace AuthService.Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

       /* public AppDbContext()
         {

         }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=productdb;User ID=sa;Password=12345;");
         }*/

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
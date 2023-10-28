using ProductService.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProductService.Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        /*  public AppDbContext()
          {

          }

          protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          {
              optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=productdb;User ID=sa;Password=12345;");
          }*/



        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Tasker> Taskers { get; set; }
        public DbSet<TaskerCert> TaskerCerts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskDetail> TaskDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(t => t.Customer!)
            .HasForeignKey(t => t.CustomerId);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId);
                entity.HasMany(d => d.OrderDetails)
                    .WithOne(p => p.Order)
                    .HasForeignKey(d => d.OrderId);
                entity.HasMany(d => d.Contracts)
                    .WithOne(p => p.Order)
                    .HasForeignKey(d => d.OrderId);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId);

                

                entity.HasOne(d => d.TaskDetail)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.TaskDetailId);
            });

            modelBuilder.Entity<TaskDetail>(entity =>
            {
                entity.HasMany(d => d.OrderDetails)
                .WithOne(p => p.TaskDetail)
                .HasForeignKey(d => d.TaskDetailId);

                entity.HasOne(d => d.Category)
                .WithMany(p => p.TaskDetails)
                .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(d => d.TaskDetails)
                .WithOne(p => p.Category)
                .HasForeignKey(d => d.CategoryId);

                entity.HasMany(d => d.TaskerCerts)
                .WithOne(p => p.Category)
                .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<TaskerCert>(entity =>
            {
                entity.HasOne(d => d.Tasker)
                .WithMany(p => p.TaskerCerts)
                .HasForeignKey(d => d.TaskerId);

                entity.HasOne(d => d.Category)
                .WithMany(p => p.TaskerCerts)
                .HasForeignKey(d => d.CategoryId);

                entity.HasKey(e => new { e.CategoryId, e.TaskerId });
            });

            modelBuilder.Entity<Tasker>(entity =>
            {
                entity.HasMany(d => d.Contracts)
                .WithOne(p => p.Tasker)
                .HasForeignKey(d => d.TaskerId);

                entity.HasMany(d => d.TaskerCerts)
                .WithOne(p => p.Tasker)
                .HasForeignKey(d => d.TaskerId);
            });

            modelBuilder.Entity<Contract>(entity =>{
                entity.HasKey(e => new { e.OrderId, e.TaskerId });

                entity.HasOne(d => d.Order)
                .WithMany(p => p.Contracts)
                .HasForeignKey(d => d.OrderId);

                entity.HasOne(d => d.Tasker)
                .WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TaskerId);
            });
        }
    }
}
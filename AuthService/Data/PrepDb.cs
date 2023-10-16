using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Entities;

namespace AuthService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }

        }

        private static void SeedData(AppDbContext context, bool isProd)
        {

            if (isProd)
            {
                Console.WriteLine("--> Attempting to appply migration...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not run Migration: {ex.Message}");
                }
            }

            if (!context.Accounts.Any())
            {
                Console.WriteLine("Seeding data...");

                context.Accounts.AddRange(
                    new Account() { PhoneNumber = "0765008474", Password = "12345", Role = Enums.Role.Customer, Balance = 100000, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = true },
                    new Account() { PhoneNumber = "0987654321", Password = "12345", Role = Enums.Role.Tasker, Balance = 200000, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = true },
                    new Account() { PhoneNumber = "0123456789", Password = "12345", Role = Enums.Role.Customer, Balance = 100000, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = true },
                    new Account() { PhoneNumber = "0908070672", Password = "12345", Role = Enums.Role.Tasker, Balance = 300000, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = true }
                );

                // context.Customers.AddRange(
                //     new Customer() { Name = "Bui Minh", Email = "buiminh@gmail.com", Address ="Vung Tau",  Status = true },
                //     new Customer() { Name = "Nguyen Huy", Email = "nguyenhuy@gmail.com", Address ="Ho Chi Minh", Status = true },
                //     new Customer() { Name = "Duc Hoang", Email = "duchoang@gmail.com", Address ="Bien Hoa", Status = true }
                // );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have data");
            }
        }
    }
}
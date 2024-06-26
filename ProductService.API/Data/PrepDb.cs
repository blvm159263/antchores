using System;
using System.Collections.Generic;
using ProductService.Repositories.Entities;
using ProductService.Services.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProductService.Repositories.Data;
using ProductService.Services.Services;

namespace ProductService.API.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder, bool isProd)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<ICustomerDataClient>();

                var customers = grpcClient.ReturnAllCustomers();

                var taskers = grpcClient.ReturnAllTaskers();

                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),
                        serviceScope.ServiceProvider.GetService<IOrderService>(),
                        customers, taskers, isProd);
            }
        }

        private static void SeedData(AppDbContext context,
                                    IOrderService orderService,
                                    IEnumerable<Customer> customers,
                                    IEnumerable<Tasker> taskers,
                                    bool isProd)
        {
            if (true)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not run migrations: {ex.Message}");
                }
            }

            //fetch customer
            if (!context.Customers.Any() && customers != null)
            {
                Console.WriteLine("--> Seeding new data Customer...");
                foreach (var cus in customers)
                {
                    if (!orderService.ExternalCustomerExists(cus.ExternalId))
                    {
                        orderService.CreateCustomer(cus);
                    }
                }
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }

            //fetch Tasker
            if (!context.Taskers.Any() && taskers != null)
            {
                Console.WriteLine("--> Seeding new data Tasker...");
                foreach (var cus in taskers)
                {
                    if (!orderService.ExternalTaskerExists(cus.ExternalId))
                    {
                        orderService.CreateTasker(cus);
                    }
                }
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }



        }
    }
}
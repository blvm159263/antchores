using System;
using System.Collections.Generic;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Repositories;
using ProductService.Services.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProductService.Repositories.Data;

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
                        serviceScope.ServiceProvider.GetService<IOrderRepository>(),
                        customers, taskers, isProd);
            }
        }

        private static void SeedData(AppDbContext context,
                                    IOrderRepository orderRepository,
                                    IEnumerable<Customer> customers,
                                    IEnumerable<Tasker> taskers,
                                    bool isProd)
        {

            if (isProd)
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
                    if (!orderRepository.ExternalCustomerExists(cus.ExternalId))
                    {
                        orderRepository.CreateCustomer(cus);
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
                    if (!orderRepository.ExternalTaskerExists(cus.ExternalId))
                    {
                        orderRepository.CreateTasker(cus);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AuthService.Services.AsyncDataServices;
using AuthService.Repositories.Data;
using AuthService.Repositories.Repositories;
using AuthService.Repositories.Repositories.Impl;
using AuthService.Services.SyncDataServices.Grpc;
using AuthService.Services.SyncDataServices.Http;
using AuthService.API.Data;
using AuthService.Services.CacheService;

namespace AuthService.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("Using SqlServer Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("AuthConn")));
            }
            else
            {
                Console.WriteLine("Using Inmem");
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            }

            services.AddStackExchangeRedisCache(opt =>
            {
                string connection = Configuration.GetConnectionString("Redis");
                opt.Configuration = connection;
            });


            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITaskerRepository, TaskerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICacheService, CacheService>();

            services.AddHttpClient<IAuthDataClient, HttpAuthDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
            });

            Console.WriteLine($"ProductService Enpoint {Configuration["ProductService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcAuthService>();

                endpoints.MapGet("/protos/customer.proto", async context =>
                {
                    await context.Response.WriteAsync(System.IO.File.ReadAllText("Protos/customer.proto"));
                });

            });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}

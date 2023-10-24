using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AuthService.Services.AsyncDataServices;
using AuthService.Repositories.Data;
using AuthService.Repositories.Repositories;
using AuthService.Services.SyncDataServices.Grpc;
using AuthService.Services.SyncDataServices.Http;
using AuthService.API.Data;
using AuthService.Services.CacheService;
using AuthService.Repositories.Models;
using AuthService.Services.Services;
using AuthService.Services.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                /*Console.WriteLine("Using Inmem");
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));*/
                Console.WriteLine("Using SqlServer Db local");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("AuthConn")));
            }
            services.AddStackExchangeRedisCache(opt =>
            {
                string connection = Configuration.GetConnectionString("Redis");
                opt.Configuration = connection;
            });

            services.Configure<JwtOptionsModel>(Configuration.GetSection("ApiSettings:JwtOptions"));

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITaskerService, TaskerService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenService, AuthenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<CustomerRepository>();
            services.AddScoped<TaskerRepository>();
            services.AddScoped<AccountRepository>();

            services.AddHttpClient<IAuthDataClient, HttpAuthDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();

            Console.WriteLine($"ProductService Enpoint {Configuration["ProductService"]}");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });

                c.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter 'Bearer' [space] and then your token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["ApiSettings:JwtOptions:Issuer"],
                        ValidAudience = Configuration["ApiSettings:JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApiSettings:JwtOptions:SecretKey"]))
                    };
                }
            );
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

            app.UseAuthentication();

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

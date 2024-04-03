using Application.Activities;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Enable API explorer for endpoints
            services.AddEndpointsApiExplorer();
            // Add database context with SQLite connection
            services.AddDbContext<DataContext>(options => options.UseSqlite(config.GetConnectionString("DefaultConnection")));
            // Register MediatR services from assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TagsList.Handler).Assembly));
            // Add TokenService to the DI container
            services.AddScoped<TokenService>();
            // Configure Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.EnableAnnotations();
            });

            return services;
        }
    }
}
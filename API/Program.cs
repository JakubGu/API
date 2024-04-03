using API.Extensions;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create a new scope and get services
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    // Get required services and apply migrations
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(userManager, context);
}
catch (Exception ex)
{
    // Log any errors
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, $"An error occurred during migration {ex.Message}\n{ex.StackTrace}");
}

app.Run();

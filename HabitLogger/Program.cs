using HabitLogger.Data;
using HabitLogger.Repository;
using HabitLogger.Services;
using HabitLogger.Views;
using Microsoft.Extensions.DependencyInjection;

// Configuring the Dependency Injection container
var services = new ServiceCollection();

// Registering the dependencies
services.AddDbContext<HabitLoggerDbContext>();
services.AddScoped<IHabitRepository, HabitRepository>();
services.AddScoped<IHabitService, HabitService>();
services.AddScoped<IProgressRepository, ProgressRepository>();
services.AddScoped<IProgressService, ProgressService>();
services.AddScoped<IMenu, Menu>();

// Building the service provider
var servicesProvider = services.BuildServiceProvider();

// Set up database
using (var scope = servicesProvider.CreateScope())
{
    var data = scope.ServiceProvider.GetRequiredService<HabitLoggerDbContext>();
    data.Database.EnsureDeleted();
    data.Database.EnsureCreated();
    SeedDatabase.SeedData(data);
}

// Get the main menu and run the app
var menu = servicesProvider.GetRequiredService<IMenu>();
menu.MainMenu();

// Dispose of the service provider
servicesProvider.Dispose();

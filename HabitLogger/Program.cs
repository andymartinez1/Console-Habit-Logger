using System.Data;
using HabitLogger.Data;
using HabitLogger.Services;
using HabitLogger.Views;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Configuring the Dependency Injection container
var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString =
    configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();

// Registering the dependencies
services.AddTransient<IDbConnection>(sp => new SqliteConnection(connectionString));
services.AddScoped<DatabaseContext>();
services.AddScoped<IHabitService, HabitService>();
services.AddScoped<IProgressService, ProgressService>();
services.AddScoped<IMenu, Menu>();

// Building the service provider
var servicesProvider = services.BuildServiceProvider();

// Set up database
using (var scope = servicesProvider.CreateScope())
{
    var data = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    data.CreateDatabase();
}

// Get the main menu and run the app
var menu = servicesProvider.GetRequiredService<IMenu>();
menu.MainMenu();

// Dispose of the service provider
servicesProvider.Dispose();

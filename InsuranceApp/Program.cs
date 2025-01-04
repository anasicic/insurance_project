using Microsoft.Data.Sqlite;
using System.Data;
using InsuranceApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger service for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("http://localhost:5000"); 

// Add database connection (SQLite) as a singleton service
builder.Services.AddSingleton<IDbConnection>(sp =>
{
    // Retrieve the connection string from configuration
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqliteConnection(connectionString); // Create a SqliteConnection with the connection string
});

// Register PartnerService and PolicyService which will use the database connection
builder.Services.AddScoped<PartnerService>();
builder.Services.AddScoped<PolicyService>();

// Add MVC support (controllers and views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Use Swagger for API documentation in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

// Enable static files (CSS, JS, images, etc.) to be served by the app
app.UseStaticFiles();

// Map default route for controllers
app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map route for creating partner 
app.MapControllerRoute(
    name: "createPartner",
    pattern: "CreatePartner/{action=Create}/{id?}");

app.Run();

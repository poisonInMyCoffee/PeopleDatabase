using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Entities;

var builder = WebApplication.CreateBuilder(args);

//Logging
builder.Host.ConfigureLogging(loggingProvider =>
{
    loggingProvider.ClearProviders();
    loggingProvider.AddConsole(); //Deleted all the default providers but only left in console,so the message won't be visible elsewhere
    loggingProvider.AddDebug(); //Now adding debug also, so it will be visible there too
});

builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));  //To show that we are using sql server 
    }
    );

//connection string
//Data Source=(localdb)\Begum;Initial Catalog=PersonsDatabase;
//Integrated Security=True;Connect Timeout=30;
//Encrypt=False;Trust Server Certificate=False;
//Application Intent=ReadWrite;Multi Subnet Failover=False

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("Warning-message");
//app.Logger.LogError("Error-message");
//app.Logger.LogCritical("Critical-message");

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
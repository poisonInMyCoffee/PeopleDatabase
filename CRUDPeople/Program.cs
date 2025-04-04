using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<PersonsDbContext>(
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

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
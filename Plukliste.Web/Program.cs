using Microsoft.EntityFrameworkCore;
using Plukliste.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PluklistDbContext>(pdbc => pdbc.UseSqlServer(connectionString), ServiceLifetime.Scoped);
builder.Services.AddDbContextFactory<PluklistDbContext>(pdbcf => pdbcf.UseSqlServer(connectionString), ServiceLifetime.Scoped);
builder.Services.AddScoped<IItemRepository, SQLItemRepository>();
builder.Services.AddScoped<IPluklisteRepository, SQLPluklisteRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();



app.Run();


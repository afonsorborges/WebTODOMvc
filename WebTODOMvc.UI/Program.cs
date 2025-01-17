using Microsoft.EntityFrameworkCore;
using WebTODOMvc.BusinessLogic.Data;
using WebTODOMvc.BusinessLogic.Services;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

logger.Information("Starting program.cs");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionWebTODOMvc")));

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<TodoDbContext>();
builder.Services.AddScoped<ToDoListService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();

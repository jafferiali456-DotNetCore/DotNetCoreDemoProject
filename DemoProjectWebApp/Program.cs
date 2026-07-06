using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Mvc.RoleAuthorization.Data;
using DemoProjectWebApp;
using DemoProjectWebApp.DAO;
using Serilog;
using System.Diagnostics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.File("logs/DemoProjectWebApp.txt", rollingInterval: RollingInterval.Day) // Log file will be created in "logs" folder
	.CreateLogger();

// Add Serilog to the logging pipeline
builder.Host.UseSerilog();  // Replace default logger with Serilog



// Add framework services.
builder.Services
	.AddControllersWithViews();
builder.Services.AddSingleton<ApplicationDataService>();
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add Kendo UI services to the services container
builder.Services.AddKendo();
builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<DemoProjectWebApp.Controllers.LoginModelController>(); // Register your service

// Add framework services.


// Add services to the container.


builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[] { new CultureInfo("en-GB") }; // or your preferred culture
	options.DefaultRequestCulture = new RequestCulture("en-GB");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();


app.UseRequestLocalization();


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

app.UseAuthorization();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started.");

app.UseSession();
app.MapControllerRoute(
		name: "areaRoute",
		pattern: "{area:exists}/{controller}/{action}",
		defaults: new { action = "Index" });


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=LoginModel}/{action=Login}/{id?}");
//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

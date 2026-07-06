using DemoProjectService.Classes;
using DemoProjectService.Controllers;
using DemoProjectService.Data;
using MicroserviesWebApplication.Generic_Repo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//////builder.WebHost.ConfigureKestrel(options =>
//////{
//////    options.ListenAnyIP(7163, listenOptions =>
//////    {
//////        listenOptions.UseHttps();  // Enforce HTTPS
//////    });
//////});


builder.Services.AddDistributedMemoryCache(); // Add a memory cache to store session data in memory
builder.Services.AddSession(options =>
{
	options.Cookie.Name = ".MyApp.Session";  // Name of the session cookie
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Session expiration time
	options.Cookie.HttpOnly = true;  // Ensure the session cookie is not accessible via JavaScript
});



builder.Services.AddDbContext<ApplicationDbContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//builder.Services.AddSingleton<LoginModelController>();
// Register specific controllers

builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.RoleRightsController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.AddUserController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.DepertmentController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.DownTimeclassController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.CountryController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.CityController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.DynamicFieldController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.PayrollFormController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ValuSetMasterController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ValuSetDetailController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.CompanyController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.DesignationController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ReligionController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.TaxSlabRateController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.GradeController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeStatusController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.DepartmentController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.FiscalYearController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.BankController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ShiftController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ReasonTypeController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.LocationController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.JobFuncController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.AttendanceController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.LeaveTypeController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeMasterController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.NationalityController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.SectsController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.DomicileController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.MartialStatusController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ContactTypeController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeSalaryController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeOrganizationController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeFamilyController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmployeeEducationController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.GroupMasterController>(); 
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.GroupDetailController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.COAController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.ScheduleTaskController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.EmpGroupProcessController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.GroupingController>();
builder.Services.AddScoped<MicroserviesWebApplication.Generic_Repo.GoalObjectiveController>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
	.WriteTo.File(builder.Configuration["Logging:LogFilePath"] ?? "logs/myapp.log", rollingInterval: RollingInterval.Day)
	.CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

builder.Services.AddAuthentication("Bearer") // Register with this name
	.AddJwtBearer("Bearer", options => // Register under this scheme
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidIssuer = "MazenWebApp",
			ValidAudience = "MazenMicroservice",
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mazen_Key_123_456_789_0000000000")),
			ValidateLifetime = true

			//  ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();
//var loggerFactory = app.Services.GetService<ILoggerFactory>();
//loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());


// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())  // Or use your own condition
{

	app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
	app.UseSwaggerUI(c =>
	{

		c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

	});
}

app.UseHttpsRedirection(); // Ensure requests are redirected to HTTPS

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();  // Enforce HTTP Strict Transport Security (HSTS)
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


app.MapControllers();
app.UseSession();


app.Run();

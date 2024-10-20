using Landing.DAL.Data;
using Landing.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure DbContext with SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

// Enable developer exception page for database-related issues.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with roles.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false; // Set to true in production
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // Add token providers for account confirmation and password reset

builder.Services.AddControllersWithViews(); // Add MVC support
builder.Services.AddRazorPages(); // Add support for Razor Pages

// Add AutoMapper support for mapping configurations.
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

// Ensure session services are added
builder.Services.AddDistributedMemoryCache(); // Add distributed memory cache for session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
	options.Cookie.HttpOnly = true; // Prevent client-side scripts from accessing cookies
	options.Cookie.IsEssential = true; // Ensure the session cookie is sent even if the user hasn't consented
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint(); // Enable migrations end point for development
}
else
{
	app.UseExceptionHandler("/Home/Error"); // Use error page in production
	app.UseHsts(); // Enable HSTS for production
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files

app.UseRouting();

// Order is important: Place UseSession before UseAuthentication and UseAuthorization
app.UseSession(); // This should be called before UseAuthentication

app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization(); // Enable authorization

// Configure routes
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages(); // Map Razor Pages

// Call this method after building the app
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	await CreateRoles(services);
}

app.Run();

// Method to create roles
static async Task CreateRoles(IServiceProvider serviceProvider)
{
	var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	string[] roleNames = { "Admin", "User" }; // Add your roles here
	IdentityResult roleResult;

	foreach (var roleName in roleNames)
	{
		var roleExist = await roleManager.RoleExistsAsync(roleName);
		if (!roleExist)
		{
			roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
		}
	}
}

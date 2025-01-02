using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Poliak_UI_WT.Data;
using Poliak_UI_WT.Services.ApiService.CategoryApiService;
using Poliak_UI_WT.Services.ApiServices.CategoryApiService;
using Poliak_UI_WT.Services.ApiServices.PhoneApiService;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MySQLConnectionLocal") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
string mySqlVersion = builder.Configuration.GetValue<string>("MySQLVersionLocal") ?? throw new InvalidOperationException("MySqlVersion not found");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>
    (
    options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    }
    ).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(
    opt =>
    {
        opt.AddPolicy("admin", p =>
        p.RequireClaim(ClaimTypes.Role, "admin"));
    });

builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();
//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<IPhoneService, MemoryPhoneService>();

builder.Services.AddHttpClient<IPhoneServiceApi, ApiPhoneService>(
    client => client.BaseAddress = new Uri("https://localhost:7002/api/phones/"));

builder.Services.AddHttpClient<ICategoryServiceApi, ApiCategoryService>(
    client => client.BaseAddress = new Uri("https://localhost:7002/api/categories/"));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/Account/Login");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/Account/Register");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


await DbInit.SetupIdentityAdmin(app);

app.Run();

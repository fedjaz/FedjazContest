using FedjazContest.Data;
using FedjazContest.Entities;
using FedjazContest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

IConfigurationSection emailSection = builder.Configuration.GetSection("EmailService");
builder.Services.AddSingleton<IEmailService, EmailService>((provider) => new EmailService(
    emailSection["Username"],
    emailSection["Password"],
    "smtp.gmail.com",
    587,
    emailSection["Url"],
    provider));

var app = builder.Build();

using(IServiceScope scope = app.Services.CreateScope())
{
    RoleManager<IdentityRole>? roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;

    if (roleManager != null)
    {
        await CreateRoles(roleManager);
    }
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();


static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
{
    if(await roleManager.FindByNameAsync("user") == null)
    {
        await roleManager.CreateAsync(new IdentityRole("user"));
    }

    if (await roleManager.FindByNameAsync("admin") == null)
    {
        await roleManager.CreateAsync(new IdentityRole("admin"));
    }
}
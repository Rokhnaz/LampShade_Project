using _0_Framework.Application;
using AccountManagement.Configuration;
using DiscountManagement.Configuration;
using DiscountManagement.Domain.CustomerDiscountAgg;
using InventoryManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using ServiceHost;
using ShopManagement.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthHelper, AuthHelper>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy =SameSiteMode.Strict;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = new PathString("/Account");
        o.LogoutPath = new PathString("/Account");
        o.AccessDeniedPath = new PathString("/AccessDenied");
    });

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
var connectionString = builder.Configuration.GetConnectionString("LampshadeDb");
ShopManagementBootstrapper.Configure(builder.Services, connectionString);
DiscountManagementBootstrapper.Configure(builder.Services, connectionString);
InventoryManagementBootstrapper.Configure(builder.Services,connectionString);
AccountManagementBootstrapper.Configure(builder.Services,connectionString);

builder.Services.AddTransient<IFileUploder, FileUploader>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();

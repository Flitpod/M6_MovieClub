using M6_MovieClub.Data;
using M6_MovieClub.Models;
using M6_MovieClub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options
        .UseSqlServer(connectionString)
        .UseLazyLoadingProxies();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<SiteUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services
    .AddAuthentication()
    .AddGoogle(opt =>
    {
        opt.ClientId = builder.Configuration["GoogleAuthProvider:ClientId"];
        opt.ClientSecret = builder.Configuration["GoogleAuthProvider:ClientSecret"];
        opt.SaveTokens = true;
    })
    .AddMicrosoftAccount(opt =>
    {
        opt.ClientId = builder.Configuration.GetValue<string>("MicrosoftAuthProvider:ClientId");
        opt.ClientSecret = builder.Configuration.GetValue<string>("MicrosoftAuthProvider:ClientSecret");
        opt.SaveTokens = true;
    });
// facebook login 
//    .AddFacebook(opt =>
//    {
//        opt.AppId = "";
//        opt.AppSecret = "";
//    });

builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

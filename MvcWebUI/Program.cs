using Business.Services;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region Culture
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Account/Users/Login";
        config.AccessDeniedPath = "/Account/Users/AccessDenied";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });
#endregion

#region Session
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(40); // 20 dakika
    //config.IOTimeout = Timeout.InfiniteTimeSpan;
});
#endregion

// Add services to the container.
// Windows Authentication
//string connectionString = "server=.\\SQLEXPRESS;database=ETrade Projem;trusted_connection=true;multipleactiveresultsets=true;trustservercertificate=true;";
// SQL Server Authentication
//string connectionString = "server=.\\SQLEXPRESS;database=ETrade Projem;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true;";
string connectionString = builder.Configuration.GetConnectionString("ETradeDb");
#region IoC Container (Inversion of Control Container)
builder.Services.AddDbContext<ETradeContext>(options => options.UseSqlServer(connectionString));
// Autofac veya Ninject
// AddScoped: istek (request) boyunca objenin referansýný (genelde interface veya abstract class) kullandýðýmýz yerde obje (somut class'tan oluþturulacak) bir kere oluþturulur ve yanýt (response) dönene kadar bu obje hayatta kalýr.
// AddSingleton: web uygulamasý baþladýðýnda objenin referansný (genelde interface veya abstract class) kullandýðýmýz yerde obje (somut class'tan oluþturulacak) bir kere oluþturulur ve uygulama çalýþtýðý (IIS üzerinden uygulama durdurulmadýðý veya yeniden baþlatýlmadýðý) sürece bu obje hayatta kalýr.
// AddTransient: istek (request) baðýmsýz ihtiyaç olan objenin referansýný (genelde interface veya abstract class) kullandýðýmýz her yerde bu objeyi new'ler.
//builder.Services.AddSingleton<ProductRepoBase, ProductRepo>();
//builder.Services.AddTransient<ProductRepoBase, ProductRepo>();

builder.Services.AddScoped<ProductRepoBase, ProductRepo>();
builder.Services.AddScoped<CategoryRepoBase, CategoryRepo>();
builder.Services.AddScoped<StoreRepoBase, StoreRepo>();

builder.Services.AddScoped<UserRepoBase, UserRepo>();

builder.Services.AddScoped<CityRepoBase, CityRepo>();
builder.Services.AddScoped<CountryRepoBase, CountryRepo>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStoreService, StoreService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICountryService, CountryService>();
#endregion

#region AppSettings
IConfigurationSection section = builder.Configuration.GetSection("AppSettings");
section.Bind(new AppSettings());
#endregion

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

#region Culture
app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // sen kimsin?

app.UseAuthorization(); // senin işlem için yetkin var mı?

#region Session
app.UseSession();
#endregion

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

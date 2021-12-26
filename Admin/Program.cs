using Microsoft.EntityFrameworkCore;
using Core.DataAccess;
using Core.Services;
using Core.Interfaces;
using Core.DataAccess.Entities;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Medias");
}).AddViewLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Language");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = Enum.GetNames<LanguageEnum>().Select(x => new CultureInfo(x)).ToList();
    options.DefaultRequestCulture = new RequestCulture(LanguageEnum.ru.ToString());
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// DI
builder.Services.AddScoped<IMediaService, MediaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(name: "language", pattern: "language");
//    endpoints.MapPost("/language", async context =>
//    {
//        var culture = context.Request.Form["culture"];
//        var returnUrl = context.Request.Query["returnUrl"];
//        context.Response.Cookies.Append(
//            CookieRequestCultureProvider.DefaultCookieName,
//            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
//            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
//        );
//        context.Response.Redirect(returnUrl);
//    });
//});

app.Run();

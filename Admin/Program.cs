using Microsoft.EntityFrameworkCore;
using Core.DataAccess;
using Core.Services;
using Core.Interfaces;
using Core.DataAccess.Entities;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Admin.Middleware;
using Microsoft.AspNetCore.Identity;

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
builder.Services.AddRazorPages()
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(Admin.AdminResource)));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = Settings.ActiveLanguages.Select(x => new CultureInfo(x.ToString())).ToList();
    options.DefaultRequestCulture = new RequestCulture(Settings.DefaultLanguage.ToString());
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.AddInitialRequestCultureProvider(new SessionRequestCultureProvider());
    options.AddInitialRequestCultureProvider(new UserClaimRequestCultureProvider());
});

// DI
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddSession();
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
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();
app.UseLanguageMiddleware();

app.MapRazorPages();

app.Run();

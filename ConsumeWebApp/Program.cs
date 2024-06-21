using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllersWithViews()
     .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<RequestLocalizationOptions>(Options =>
{
    var supportedculture = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE"),
        new CultureInfo("kn-IN"),
        new CultureInfo("mr-IN"),
    };
    Options.DefaultRequestCulture = new RequestCulture("en-US");
    //Options.DefaultRequestCulture = new RequestCulture("");
    Options.SupportedUICultures = supportedculture;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Set the session timeout to 2 hours
});
var app = builder.Build();
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddConsole() // Use console logger for development
        .AddDebug();   // Use debug logger for development
});
var logger = loggerFactory.CreateLogger<Program>();
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});
app.Run();

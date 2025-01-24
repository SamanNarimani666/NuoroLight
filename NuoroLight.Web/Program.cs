using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using nuorolight.web;
using NuoroLight.IOC;
using NuoroLight.Web.BackgroundServices;
using NuoroLight.Web.Extensions;
using NuoroLight.Web.Hubs;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MqttService>();

builder.Services.AddSignalR(); // اضافه کردن SignalR

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});
#endregion

#region IOC
// Add services to the container.
builder.Services.RegisterServiceInfrastructure();
DependencyContainer.RegisterServices(builder.Services, builder.Configuration.GetConnectionString("NuoroLight"));
#endregion

#region Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("NuoroLight"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));
#endregion

builder.Services.AddResponseCaching();

builder.Services.AddHangfireServer();

#region Html Encoder
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
#endregion


builder.Services.AddHostedService<MqttWorker>();  // Worker را به سرویس‌ها اضافه می‌کنیم

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

#region Handel Error404
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        if (context.Request.Path.Value.Contains("/Admin"))
        {
            context.Request.Path = "/Admin/error-404";
        }
        else
        {
            context.Request.Path = "/Home/Error404";
        }
        await next();
    }
});
#endregion

app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseCors("MyPolicy");

app.UseHangfireDashboard();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
});

// تنظیمات مسیر پیش‌فرض
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// تنظیمات مربوط به SignalR
app.MapHub<MqttHub>("/mqttHub");  // اینجا SignalR را تنظیم می‌کنیم

app.Run();

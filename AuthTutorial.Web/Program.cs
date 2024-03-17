using AuthTutorial;
using AuthTutorial.Components;
using AuthTutorial.Http;
using AuthTutorial.Services;
using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost
            .UseKestrel((context, option) => { option.Configure(context.Configuration.GetSection("Kestrel")); })
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                var env = context.HostingEnvironment.EnvironmentName;

                configBuilder.AddJsonFile("appsettings.json");
                configBuilder.AddJsonFile($"appsettings.{env}.json", optional: true);
                configBuilder.AddJsonFile($"Configs/connectionStrings.{env}.json", optional: true);

                configBuilder.AddEnvironmentVariables();
            });

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddSingleton<IAdminAuthHttpClientOptions, AppSettings>();


        builder.Services.AddHttpClient<IAdminAuthHttpCLient, AdminAuthHttpCLient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IAdminAuthHttpClientOptions>();

            client.BaseAddress = new Uri(settings.AdminAuthClientOptions.Url);
            client.Timeout = TimeSpan.FromMilliseconds(settings.AdminAuthClientOptions.TimeOutMilliseconds);
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/login";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                options.AccessDeniedPath = "/access-denied";
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
            });

        builder.Services.AddScoped<CustomCookieAuthenticationEvents>();

        builder.Services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDbContext"));
            options.UseSnakeCaseNamingConvention();
        });
        
        builder.Services.AddScoped<ICookieAuthenticationManager, CookieAuthenticationManager>();

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCascadingAuthenticationState();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
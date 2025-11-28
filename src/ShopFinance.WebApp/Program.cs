using BitzArt.Blazor.Auth.Server;
using BitzArt.Blazor.Cookies;
using Microsoft.Extensions.FileProviders;
using ShopFinance.Application;
using ShopFinance.Infrastructure;
using ShopFinance.Infrastructure.Services;
using ShopFinance.WebApp;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.AddBlazorCookies();
builder.AddBlazorAuth();
builder.Services.AddScoped<JwtService2>();


builder.AddBlazorCookies();
builder.AddBlazorAuth<CustomAuthenticationService>();
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebApp(builder.Configuration);
var app = builder.Build();

app.UseWebApp(builder.Configuration, builder.Environment);


app.MapAuthEndpoints();

app.Run();


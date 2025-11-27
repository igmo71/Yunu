using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Yunu.Api.Application;
using Yunu.Api.Application.YunuAuth;
using Yunu.Api.Endpoints;
using Yunu.Api.Infrastructure.Data;

namespace Yunu.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddProblemDetails();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection Not Found");

        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

        builder.Services.Configure<YunuConfig>(builder.Configuration.GetSection(YunuConfig.Section));

        builder.Services.AddSingleton<IYunuAuthService, AuthService>();

        builder.Services.AddTransient<AuthHeaderHandler>();

        builder.Services.AddHttpClient<IYunuClient, YunuClient>()
            .AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services.AddAppServices();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                // Disable default fonts to avoid download unnecessary fonts
                options.DefaultFonts = false;
            });
            app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapYunuAuthApi();
        app.MapAppApi();

        app.Run();
    }
}

using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Infrastructure.Persistance.Interceptors;
using Infrastructure.Persistance;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Authentication;
using Quartz;
using Zitadel.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfractructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddPersistance(configuration)
            .AddAuth(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services)
    {
        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    public static IServiceCollection AddPersistance(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

        services.AddScoped<PublishDomainEventsInterceptor>();

        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        const string defaultScheme = "ZITADEL_BASIC";

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = defaultScheme;
                options.DefaultSignInScheme = defaultScheme;
                options.DefaultSignOutScheme = defaultScheme;
                options.DefaultChallengeScheme = defaultScheme;
                options.DefaultForbidScheme = defaultScheme;
            })
            .AddZitadelIntrospection(
                defaultScheme,
                options =>
                {
                    options.Authority = "https://eloauth.com";
                    options.ClientId = "170102032621961473@library";
                    options.ClientSecret = "KNkKW8nx3rlEKOeHNUcPx80tZTP1uZTjJESfdA3kMEK7urhX3ChFukTMQrtjvG70";
                });
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateActor = true,
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        RequireExpirationTime = true,
            //        ValidIssuer = jwtSettings.Issuer,
            //        ValidAudience = jwtSettings.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            //    };
            //});

        services.AddAuthorization();

        return services;
    }
}

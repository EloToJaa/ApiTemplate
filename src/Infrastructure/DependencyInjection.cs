using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Infrastructure.Persistance.Interceptors;
using Infrastructure.Persistance;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Authentication;
using Quartz;
using Zitadel.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfractructure(
        this IServiceCollection services,
        IConfiguration configuration)
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
        IConfiguration configuration)
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
        IConfiguration configuration)
    {
        services.AddOptions<ZitadelSettings>()
            .Bind(configuration.GetSection(ZitadelSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var zitadelSettings = new ZitadelSettings();
        configuration.Bind(ZitadelSettings.SectionName, zitadelSettings);

        const string defaultScheme = ZitadelSettings.SchemeName;

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
                    options.Authority = zitadelSettings.Authority;
                    options.ClientId = zitadelSettings.ClientId;
                    options.ClientSecret = zitadelSettings.ClientSecret;
                });

        services.AddAuthorization();

        return services;
    }
}

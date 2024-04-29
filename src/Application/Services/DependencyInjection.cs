using Diacritics;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.Slugify;
using Application.Services.Image;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddImages(configuration);

        services.AddSingleton<IDiacriticsMapper, DefaultDiacriticsMapper>();
        services.AddSingleton<ISlugifyService, SlugifyService>();

        return services;
    }
}


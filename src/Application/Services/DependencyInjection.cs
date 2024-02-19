using Diacritics;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.Slugify;

namespace Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDiacriticsMapper, DefaultDiacriticsMapper>();
        services.AddSingleton<ISlugifyService, SlugifyService>();

        return services;
    }
}


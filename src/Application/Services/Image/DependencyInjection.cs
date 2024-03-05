using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Settings;

namespace Application.Services.Image;

public static class DependencyInjection
{
    public static IServiceCollection AddImages(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<ImagesSettings>(configuration.GetSection(ImagesSettings.SectionName));

        services.AddHttpClient<IImageService, ImageService>();
        services.AddTransient<IImageService, ImageService>();

        return services;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Settings;
using Refit;
using System.Net.Http.Headers;

namespace Application.Services.Image;

public static class DependencyInjection
{
    public static IServiceCollection AddImages(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ImagesSettings>()
            .Bind(configuration.GetSection(ImagesSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var imagesSettings = new ImagesSettings();
        configuration.Bind(ImagesSettings.SectionName, imagesSettings);

        services.AddRefitClient<IImageService>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri($"https://api.cloudflare.com/client/v4/accounts/{imagesSettings.AccountId}/images/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", imagesSettings.ApiToken);
            });

        return services;
    }
}

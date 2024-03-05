using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Services.Image.Response;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Quizer.Application.Common.Settings;

namespace Application.Services.Image;

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly HttpClient _client;
    private readonly ImagesSettings _settings;

    public ImageService(ILogger<ImageService> logger, HttpClient client, IOptions<ImagesSettings> imageOptions)
    {
        _settings = imageOptions.Value;
        _logger = logger;
        _client = client;

        _client.BaseAddress = new Uri($"https://api.cloudflare.com/client/v4/accounts/{_settings.AccountId}/images/");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiToken);
    }

    public async Task<ErrorOr<DirectUploadResponse>> DirectUpload(bool requireSignedURLs = false)
    {
        var formData = new MultipartFormDataContent
        {
            { new StringContent(requireSignedURLs.ToString().ToLower()), "requireSignedURLs" }
        };

        var response = await _client.PostAsync("v2/direct_upload", formData);

        _logger.LogInformation("Direct upload response: {@response}", response);

        if (!response.IsSuccessStatusCode)
            return Errors.Image.CannotUpload;

        var content = await response.Content.ReadAsStringAsync();
        if(content is null)
            return Errors.Image.CannotUpload;

        var result = JsonSerializer.Deserialize<DirectUploadResponse>(content);
        if (result is null || !result.Success)
            return Errors.Image.CannotUpload;

        return result;
    }

    public async Task<bool> IsSuccessfulyUploaded(Guid imageId)
    {
        var response = await _client.GetFromJsonAsync<ImageDetailsResponse>($"v1/{imageId}");

        if(response is null || response.Result is null || !response.Success) return false;

        return !response.Result.Draft;
    }

    public async Task<bool> DeleteImage(Guid imageId)
    {
        var response = await _client.DeleteFromJsonAsync<DeleteImageResponse>($"v1/{imageId}");

        if (response is null) return false;

        return response.Success;
    }

    public Uri GenerateImageUrl(Guid imageId, string variantName) =>
        new Uri($"https://imagedelivery.net/{_settings.AccountHash}/{imageId}/{variantName}");
}

using Application.Services.Image.Response;
using Refit;

namespace Application.Services.Image;

public interface IImageService
{
    [Post("v2/direct_upload")]
    Task<DirectUploadResponse?> DirectUpload([Body(BodySerializationMethod.UrlEncoded)] bool requireSignedURLs = false);

    [Get("v1/{imageId}")]
    Task<ImageDetailsResponse?> GetImage(Guid imageId);

    [Delete("v1/{imageId}")]
    Task<DeleteImageResponse?> DeleteImage(Guid imageId);

    Uri GenerateImageUrl(string accountHash, Guid imageId, string variantName) =>
        new Uri($"https://imagedelivery.net/{accountHash}/{imageId}/{variantName}");
}
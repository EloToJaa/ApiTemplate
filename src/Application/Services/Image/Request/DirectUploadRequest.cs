using Refit;

namespace Application.Services.Image.Request;

public class DirectUploadRequest
{
    [AliasAs("expiry")]
    public DateTime? Expiry { get; set; }

    [AliasAs("id")]
    public Guid? Id { get; set; }

    [AliasAs("metadata")]
    public object? Metadata { get; set; }

    [AliasAs("requireSignedURLs")]
    public bool RequireSignedURLs { get; set; } = false;
}

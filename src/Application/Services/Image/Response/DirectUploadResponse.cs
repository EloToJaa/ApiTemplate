using System.Text.Json.Serialization;

namespace Application.Services.Image.Response;

public class DirectUploadResponse
{
    [JsonPropertyName("result")]
    public required DirectUploadResponseResult Result { get; set; }

    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("errors")]
    public List<MessageDetails> Errors { get; set; } = new();

    [JsonPropertyName("messages")]
    public List<MessageDetails> Messages { get; set; } = new(); 
}

public class DirectUploadResponseResult
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("uploadURL")]
    public required Uri UploadUrl { get; set; }
}

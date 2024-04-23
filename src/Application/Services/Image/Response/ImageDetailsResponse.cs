using System.Text.Json.Serialization;

namespace Application.Services.Image.Response;

public class ImageDetailsResponse
{
    [JsonPropertyName("errors")]
    public List<MessageDetails> Errors { get; set; } = new();

    [JsonPropertyName("messages")]
    public List<MessageDetails> Messages { get; set; } = new();

    [JsonPropertyName("result")]
    public ImageDetailsResponseResult? Result { get; set; }

    [JsonPropertyName("success")]
    public required bool Success { get; set; }
}

public class ImageDetailsResponseResult
{
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("meta")]
    public Dictionary<string, string> Meta { get; set; } = new();

    [JsonPropertyName("requireSignedURLs")]
    public required bool RequireSignedUrls { get; set; }

    [JsonPropertyName("uploaded")]
    public required DateTime Uploaded { get; set; }

    [JsonPropertyName("variants")]
    public List<Uri> Variants { get; set; } = new();

    [JsonPropertyName("draft")]
    public bool Draft { get; set; } = false;
}
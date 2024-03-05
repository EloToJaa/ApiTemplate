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

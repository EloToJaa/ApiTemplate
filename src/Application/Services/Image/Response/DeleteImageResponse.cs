using Application.Services.Image.Response;
using System.Text.Json.Serialization;

public class DeleteImageResponse
{
    [JsonPropertyName("errors")]
    public List<MessageDetails> Errors { get; set; } = new();

    [JsonPropertyName("messages")]
    public List<MessageDetails> Messages { get; set; } = new();

    [JsonPropertyName("result")]
    public Dictionary<string, string> Result { get; set; } = new();

    [JsonPropertyName("success")]
    public required bool Success { get; set; }
}
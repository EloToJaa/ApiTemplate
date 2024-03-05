namespace Api.Common.Settings;

public class SwaggerSettings
{
    public const string SectionName = "Swagger";
    public bool Enabled { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
}

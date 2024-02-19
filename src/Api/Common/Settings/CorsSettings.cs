namespace Api.Common.Settings;

public class CorsSettings
{
    public const string SectionName = "Cors";
    public const string PolicyName = "cors";
    public string[] Origins { get; init; } = [];
}

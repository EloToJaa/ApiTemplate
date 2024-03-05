namespace Quizer.Application.Common.Settings;

public class ImagesSettings
{
    public const string SectionName = "Images";
    public string AccountId { get; init; } = null!;
    public string ApiToken { get; init; } = null!;
    public string AccountHash { get; init; } = null!;
}

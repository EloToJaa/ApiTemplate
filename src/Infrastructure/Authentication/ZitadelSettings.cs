namespace Infrastructure.Authentication;

public class ZitadelSettings
{
    public const string SectionName = "ZitadelSettings";
    public const string SchemeName = "ZITADEL_BASIC";
    public string Authority { get; init; } = null!;
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
}

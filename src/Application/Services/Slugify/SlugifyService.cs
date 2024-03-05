using System.Text.RegularExpressions;
using System.Text;
using Diacritics;

namespace Application.Services.Slugify;

public class SlugifyService : ISlugifyService
{
    protected readonly Config _config;
    protected readonly IDiacriticsMapper _diacriticsMapper;

    public SlugifyService(IDiacriticsMapper diacriticsMapper)
    {
        _config = new Config();
        _diacriticsMapper = diacriticsMapper;
    }

    public string GenerateSlug(string str)
    {
        if (_config.ForceLowerCase)
            str = str.ToLower();

        str = CleanWhiteSpace(str, _config.CollapseWhiteSpace);
        str = ApplyReplacements(str, _config.CharacterReplacements);
        str = _diacriticsMapper.RemoveDiacritics(str);
        str = DeleteCharacters(str, _config.DeniedCharactersRegex);

        return str;
    }

    protected string CleanWhiteSpace(string str, bool collapse)
    {
        return Regex.Replace(str, collapse ? @"\s+" : @"\s", " ");
    }

    protected string ApplyReplacements(string str, Dictionary<string, string> replacements)
    {
        StringBuilder sb = new StringBuilder(str);

        foreach (KeyValuePair<string, string> replacement in replacements)
            sb.Replace(replacement.Key, replacement.Value);

        return sb.ToString();
    }

    protected string DeleteCharacters(string str, string regex)
    {
        return Regex.Replace(str, regex, "");
    }

    public class Config
    {
        public Dictionary<string, string> CharacterReplacements { get; set; }
        public bool ForceLowerCase { get; set; }
        public bool CollapseWhiteSpace { get; set; }
        public string DeniedCharactersRegex { get; set; }

        public Config()
        {
            CharacterReplacements = new Dictionary<string, string>
            {
                { " ", "-" }
            };

            ForceLowerCase = true;
            CollapseWhiteSpace = true;
            DeniedCharactersRegex = @"[^a-zA-Z0-9\-\._]";
        }
    }
}

using Core.Enums;

namespace Core
{
    public static class Settings
    {
        public static Language DefaultLanguage => Language.ru;
        public static IEnumerable<Language> ActiveLanguages => Enum.GetValues<Language>();
    }
}

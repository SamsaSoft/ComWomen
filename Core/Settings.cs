using Core.Enums;

namespace Core
{
    public static class Settings
    {
        public static LanguageEnum DefaultLanguage => LanguageEnum.ru;
        public static IEnumerable<LanguageEnum> ActiveLanguages => Enum.GetValues<LanguageEnum>();
    }
}

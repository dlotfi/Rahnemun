using Edreamer.Framework.Settings;

namespace Rahnemun.Layout.ThemeManager
{
    public static class Extensions
    {
        public static string GetThemeName(this ISettingsService settingsService)
        {
            return settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunStyle", Name = "ThemeName" });
        }
    }
}
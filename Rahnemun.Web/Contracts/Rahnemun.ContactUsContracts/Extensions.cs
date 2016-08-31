using Edreamer.Framework.Settings;

namespace Rahnemun.ContactUsContracts
{
    public static class Extensions
    {
        public static string GetContactTelNo(this ISettingsService settingsService)
        {
            return settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunContact", Name = "TelNo" });
        }

        public static string GetContactTelTitle(this ISettingsService settingsService)
        {
            return settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunContact", Name = "TelTitle" });
        }
    }
}

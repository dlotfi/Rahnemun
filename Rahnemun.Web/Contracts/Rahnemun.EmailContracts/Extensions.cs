using Edreamer.Framework.Settings;

namespace Rahnemun.EmailContracts
{
    public static class Extensions
    {
        public static string GetNoReplyEmail(this ISettingsService settingsService)
        {
            return settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunEmail", Name = "NoReplyAddress" });
        }

        public static string GetSupportEmail(this ISettingsService settingsService)
        {
            return settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunEmail", Name = "SupportAddress" });
        }

        public static string GetDefaultSenderName(this ISettingsService settingsService)
        {
            return settingsService.GetSetting<string>(new SettingEntryKey { Category = "RahnemunEmail", Name = "DefaultSenderName" });
        }
    }
}

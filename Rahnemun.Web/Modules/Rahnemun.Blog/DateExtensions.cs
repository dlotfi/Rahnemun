using System;
using Rahnemun.Common;

namespace Rahnemun.Blog
{
    public static class DateExtension
    {
        public static string ToLocalFormattedTime(this DateTime time, IDateTimeLocalizationService dateTimeLocalizationService)
        {
            const int minute = 60;
            const int hour = 60*minute;

            var localTime = dateTimeLocalizationService.ConvertToUserTimeZone(time);
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - time.Ticks);
            var seconds = ts.TotalSeconds;

            if (seconds < 2*minute)
                return "یک دقیقه قبل";
            if (seconds < 60*minute)
                return ts.Minutes + " دقیقه قبل";
            if (seconds < 120*minute)
                return "یک ساعت قبل";
            if (seconds < 24*hour)
                return ts.Hours + " ساعت قبل";
            if (seconds < 48*hour)
                return "دیروز " + localTime.ToString("h:mm");
            return localTime.ToString("dddd،dd MMMM h:mm");
        }
    }
}
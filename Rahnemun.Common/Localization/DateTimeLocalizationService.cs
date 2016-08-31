using System;

namespace Rahnemun.Common
{
    public class DateTimeLocalizationService: IDateTimeLocalizationService
    {
        public DateTime ConvertToUserTimeZone(DateTime dateTime, int? userId = null)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            //userId = userId ?? _workContextAccessor.Context.CurrentUser()?.Id;
            //ToDo-Low [08221543]: Store each user's preferred time zone and convert dateTime to it.
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Iran Standard Time");
        }
    }
}
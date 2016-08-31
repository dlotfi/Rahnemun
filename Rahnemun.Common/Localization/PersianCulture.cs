using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Rahnemun.Common
{
    public class PersianCulture : CultureInfo
    {

        private readonly Calendar _cal;

        private readonly Calendar[] _optionals;

        public PersianCulture()
            : base("fa-IR", true)
        {
            _cal = base.OptionalCalendars[0]; // Somehow, this line is required!
            var optionalCalendars = new List<Calendar>();
            optionalCalendars.AddRange(base.OptionalCalendars);
            optionalCalendars.Insert(0, new PersianCalendar());
            var formatType = typeof(DateTimeFormatInfo);
            var calendarType = typeof(Calendar);
            var idProperty = calendarType.GetProperty("ID", BindingFlags.Instance | BindingFlags.NonPublic);
            var optionalCalendarfield = formatType.GetField("optionalCalendars", BindingFlags.Instance | BindingFlags.NonPublic);
            var newOptionalCalendarIDs = new Int32[optionalCalendars.Count];
            
            for (int i = 0; i < newOptionalCalendarIDs.Length; i++)
                newOptionalCalendarIDs[i] = (Int32)idProperty.GetValue(optionalCalendars[i], null);
            optionalCalendarfield.SetValue(DateTimeFormat, newOptionalCalendarIDs);

            _optionals = optionalCalendars.ToArray();
            _cal = _optionals[0];
            DateTimeFormat.Calendar = _cal;
            SetOtherPersianDateTimeFormatInfo(DateTimeFormat);
        }

        public override Calendar Calendar
        {
            get { return _cal; }
        }

        public override Calendar[] OptionalCalendars
        {
            get { return _optionals; }
        }

        // Based on Orchard (version 1.9 or later)
        private static void SetOtherPersianDateTimeFormatInfo(DateTimeFormatInfo formatInfo)
        {
            var persianCalendarMonthNames = new[] {
                "فررودین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر",
                "آبان",
                "آذر",
                "دی",
                "بهمن",
                "اسفند",
                "" // 13 months names always necessary...
            };

            formatInfo.MonthNames =
                formatInfo.AbbreviatedMonthNames =
                formatInfo.MonthGenitiveNames =
                formatInfo.AbbreviatedMonthGenitiveNames =
                persianCalendarMonthNames;

            var persianDayNames = new[] {
                "یکشنبه", // Changes the Arabic "ي" and "ك" to the Farsi "ی" and "ک" respectively (incorrect in .NET Framework).
                "دوشنبه",
                "سه شنبه",
                "چهارشنبه",
                "پنجشنبه",
                "جمعه",
                "شنبه"
            };

            formatInfo.DayNames =
                formatInfo.AbbreviatedDayNames =
                persianDayNames;

            formatInfo.SetAllDateTimePatterns(new[] {
                "yyyy/MM/dd",
                "yy/MM/dd",
                "yyyy/M/d",
                "yy/M/d"
            }, 'd');

            formatInfo.SetAllDateTimePatterns(new[] {
                "dddd، d MMMM yyyy",
                "d MMMM yyyy"
            }, 'D');

            formatInfo.SetAllDateTimePatterns(new[] {
                "MMMM yyyy",
                "MMMM yy"
            }, 'y');

            formatInfo.SetAllDateTimePatterns(new[] {
                "HH:mm",
                "H:mm",
                "hh:mm tt",
                "h:mm tt"
            }, 't');

            formatInfo.SetAllDateTimePatterns(new[] {
                "HH:mm:ss",
                "H:mm:ss",
                "hh:mm:ss tt",
                "h:mm:ss tt"
            }, 'T');
        }
    }
}
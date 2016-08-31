using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Rahnemun.Common
{
    public static class FormatExtensions
    {
        public static object Errors(this ModelStateDictionary modelStateDictionary, string prefix = null, string separator = "\r\n", string bullet = "• ")
        {
            dynamic errors = new ExpandoObject();
            foreach (var item in modelStateDictionary)
            {
                var message = FormatErrorMessage(item.Value.Errors, separator, bullet);
                if (String.IsNullOrEmpty(message)) continue;
                var name = item.Key == "" ? "" : (prefix ?? "") + item.Key;
                ((IDictionary<String, Object>)errors).Add(name, message);
            }
            return errors;
        }

        private static string FormatErrorMessage(IEnumerable<ModelError> errors, string separator, string bullet)
        {
            var sb = new StringBuilder();
            foreach (var error in errors)
            {
                var message = String.IsNullOrEmpty(error.ErrorMessage)
                                  ? error.Exception.Message
                                  : error.ErrorMessage;
                if (bullet != null) sb.Append(bullet);
                sb.Append(message);
                if (separator != null) sb.Append(separator);
            }
            if (errors.Count() == 1 && bullet != null)
            {
                sb.Remove(0, bullet.Length);
            }
            if (errors.Any() && separator != null)
            {
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }
            return sb.ToString();
        }

        public static string ToIsoTime(this DateTime time)
        {
            return time.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
    }
}
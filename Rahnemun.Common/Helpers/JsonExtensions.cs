using System.Web.Mvc;
using Edreamer.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Rahnemun.Common
{
    public static class JsonExtensions
    {
        public static string ToFormattedJson(this HtmlHelper html, object data, bool autoCamelCase = true)
        {
            Throw.IfArgumentNull(data, "data");
            var serializerSettings = new JsonSerializerSettings
                                     {
                                         Converters = new[] { new StringEnumConverter() /*new IsoDateTimeConverter()*/ },
                                         DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                     };
            if (autoCamelCase)
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.SerializeObject(data, Formatting.None, serializerSettings);
        }

        public static T DeserializeJson<T>(this string json)
        {
            Throw.IfArgumentNull(json, "json");
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

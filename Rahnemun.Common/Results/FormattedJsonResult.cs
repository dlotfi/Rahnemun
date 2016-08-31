using System.Text;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Rahnemun.Common
{
    public class FormattedJsonResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        private readonly JsonSerializerSettings _serializerSettings;
        private readonly Formatting _formatting;

        public FormattedJsonResult(object data, bool autoCamelCase = true)
        {
            _serializerSettings = new JsonSerializerSettings
                                  {
                                      Converters = new[] { new StringEnumConverter() /*new IsoDateTimeConverter()*/},
                                      DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                  };
            if (autoCamelCase)
                _serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _formatting = Formatting.None;
            Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Throw.IfArgumentNull(context, "context");

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = _formatting };
                var serializer = JsonSerializer.Create(_serializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}
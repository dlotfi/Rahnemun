using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Rahnemun.Common
{
    public static class ValidationMessageExtensions
    {
        private const string WrapperTag = "span";

        public static IHtmlString WrappedValidationMessage(this HtmlHelper htmlHelper, string modelName, object htmlAttributes)
        {
            var validationElement = htmlHelper.ValidationMessage(modelName, htmlAttributes).ToString();
            return new HtmlString(WrapInnerText(validationElement, htmlHelper.ViewContext.ValidationMessageElement, WrapperTag));
        }

        private static string WrapInnerText(string element, string tag, string wrapperTag)
        {
            return Regex.Replace(element, "(?<=>)(.+)(?=</" + tag + ")", 
                match =>
                {
                    //var last = match.NextMatch().Index == 0;
                    //if (!last) return match.Value;
                    var builder = new TagBuilder(wrapperTag) { InnerHtml = match.Value };
                    return builder.ToString(TagRenderMode.Normal);
                },
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }
}

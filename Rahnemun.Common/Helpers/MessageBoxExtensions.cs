using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Edreamer.Framework.Helpers;

namespace Rahnemun.Common
{
    public static class MessageBoxExtensions
    {
        public static IHtmlString MessageBoxInfo(this HtmlHelper htmlHelper, string message, string title = "اطلاعات", bool animate = false)
        {
            return MessageBox(htmlHelper, message, null, title, MessageBoxType.Info, MessageBoxMode.Auto, animate);
        }

        public static IHtmlString MessageBoxSuccess(this HtmlHelper htmlHelper, string message, string title = "موفقیت", bool animate = false)
        {
            return MessageBox(htmlHelper, message, null, title, MessageBoxType.Success, MessageBoxMode.Auto, animate);
        }

        public static IHtmlString MessageBoxWarning(this HtmlHelper htmlHelper, string message, string title = "اخطار", bool animate = false)
        {
            return MessageBox(htmlHelper, message, null, title, MessageBoxType.Warning, MessageBoxMode.Auto, animate);
        }

        public static IHtmlString MessageBoxError(this HtmlHelper htmlHelper, string message, string title = "خطا", bool animate = false)
        {
            return MessageBox(htmlHelper, message, null, title, MessageBoxType.Error, MessageBoxMode.Auto, animate);
        }

        public static IHtmlString MessageBox(this HtmlHelper htmlHelper, string message, string messageHtml, string title, MessageBoxType type, MessageBoxMode mode, bool animate, object htmlAttributes = null)
        {
            Throw.If(String.IsNullOrEmpty(message) && String.IsNullOrEmpty(messageHtml))
                .AnArgumentException("At least one of message or messageHtml should be non-empty.");
            message = message ?? String.Empty;
            messageHtml = messageHtml ?? String.Empty;

            var div = new TagBuilder("div");
            div.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            switch (type)
            {
                case MessageBoxType.Info: div.AddCssClass("status"); break;
                case MessageBoxType.Success: div.AddCssClass("status-success"); break;
                case MessageBoxType.Error: div.AddCssClass("status-error"); break;
                case MessageBoxType.Warning: div.AddCssClass("status-warning"); break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, "Specified MessageBoxType is not valid.");
            }
            if (!animate)
                div.AddCssClass("status-visible");

            var divHtml = new StringBuilder();
            divHtml.Append("<a class=\"close\" title=\"بستن\" href=\"#\">&times;</a>");
            var messageParagraphs = message.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (mode == MessageBoxMode.Auto)
                mode = (messageParagraphs.Count() > 1 || message.Length > 150 || !String.IsNullOrEmpty(messageHtml)) ? MessageBoxMode.Expanded : MessageBoxMode.Inline;

            if (mode == MessageBoxMode.Expanded)
            {
                if (!String.IsNullOrEmpty(title))
                    divHtml.Append($"<h4>{title}</h4>");
                foreach (var paragraph in messageParagraphs)
                {
                    divHtml.Append($"<p>{paragraph}</p>");
                }
            }
            else // mode == MessageBoxMode.Inline
            {
                divHtml.Append("<p>");
                if (!String.IsNullOrEmpty(title))
                    divHtml.Append($"<strong>{title}:</strong>&nbsp;");
                divHtml.Append(message + "</p>");
            }
            divHtml.Append(messageHtml);

            div.InnerHtml = divHtml.ToString();
            return new HtmlString(div.ToString());
        }

        public static IHtmlString ValidationSummaryBox(this HtmlHelper htmlHelper, bool excludePropertyErrors)
        {
            if (htmlHelper.ViewData.ModelState.IsValid)
                return new HtmlString("");

            IEnumerable<ModelState> modelStates = null;
            if (excludePropertyErrors)
            {
                ModelState ms;
                htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, out ms);
                if (ms != null)
                    modelStates = new[] {ms};
            }
            else
            {
                modelStates = htmlHelper.ViewData.ModelState.Values;
            }
            if (modelStates == null)
                return new HtmlString("");
            
            var modelErrors = modelStates
                .SelectMany(m => m.Errors)
                .Where(me => !String.IsNullOrEmpty(me.ErrorMessage))
                .ToList();

            if (modelErrors.Count() == 1)
                return MessageBoxError(htmlHelper, modelErrors.First().ErrorMessage);

            var ulTag = new TagBuilder("ul");
            var ulTagHtml = new StringBuilder();
            foreach (var modelError in modelErrors)
                ulTagHtml.Append($"<li>{modelError.ErrorMessage}</li>");
            ulTag.InnerHtml += ulTagHtml;
            return MessageBox(htmlHelper, null, ulTag.ToString(), "خطا", MessageBoxType.Error, MessageBoxMode.Expanded, false);
        }
    }

    public enum MessageBoxType
    {
        Info,
        Success,
        Error,
        Warning
    }

    public enum MessageBoxMode
    {
        Auto,
        Expanded,
        Inline
    }
}
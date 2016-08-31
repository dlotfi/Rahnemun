using System.Collections.Generic;
using System.Web;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.UI.Resources;
using Rahnemun.Common;
using Rahnemun.HomeContracts;

namespace Rahnemun.Home
{
    public class CategoryHomeWidgetProvider : IHomeWidgetProvider
    {
        private readonly IContentLocator _contentLocator;

        public CategoryHomeWidgetProvider(IContentLocator contentLocator)
        {
            _contentLocator = contentLocator;
        }

        public IEnumerable<HomeSlideWidgetModel> GetHomeSlideWidgets()
        {
            yield return new HomeSlideWidgetModel
            {
                Priority = 1000,
                Html = html =>
                       {
                           var aboutUrl = html.GetUrlHelper().Route<IAboutRoute>().Get();
                           return new HtmlString(
                               "<h2>مشاوره آنلاین رهنمون</h2>" +
                               "<p>رهنمون یک موسسه مشاوره آنلاین است که به یاری مشاوران متخصص خود با هزینه مناسب و به آسانی شما را برای پاسخ به چالشهای زندگی راهنمایی می کند.</p>" +
                               $"<a class='button default' title='درباره رهنمون' href='{aboutUrl}'>درباره رهنمون</a>");
                       },
                ImageUrl = url => _contentLocator.GetContentUrl(GetType(), "~/Images/about-slide.jpg")
            };
        }

        public IEnumerable<HomeWidgetModel> GetHomeIntroWidgets()
        {
            yield return new HomeWidgetModel
            {
                Priority = 1000,
                Html = html =>
                       {
                           var howUrl = html.GetUrlHelper().Route<IHowItWorksRoute>().Get();
                           var howIntroImage = _contentLocator.GetContentUrl(GetType(), "~/Images/how-intro.jpg");
                           return new HtmlString(
                               $"<a title='چرا رهنمون؟' href='{howUrl}'><img src='{howIntroImage}' alt='چرا رهنمون؟'></a>" +
                               "<div class='contents'>" +
                               $"<header><h3><a title='چرا رهنمون؟' href='{howUrl}'>چرا رهنمون؟</a></h3></header>" +
                               "<p>رهنمون با ارایه خدمات مشاوره در حوزه های گوناگون زندگی و دارا بودن مشاوران متخصص، این امکان را فراهم می سازد تا در هر زمان و در هر منطقه جغرافیایی که هستید از طریق جلسات راهنمایی و مشاوره آنلاین، با افراد متخصص گفتگو نمایید و نیازهای خود را مرتفع سازید.</p>" +
                               "</div>");
                       }
            };

            yield return new HomeWidgetModel
            {
                Priority = 500,
                Html = html =>
                       {
                           var aboutUrl = html.GetUrlHelper().Route<IAboutRoute>().Get();
                           var aboutIntroImage = _contentLocator.GetContentUrl(GetType(), "~/Images/about-intro.jpg");
                           return new HtmlString(
                               $"<a title='درباره رهنمون' href='{aboutUrl}'><img src='{aboutIntroImage}' alt='درباره رهنمون'></a>" +
                               "<div class='contents'>" +
                               $"<header><h3><a title='درباره رهنمون' href='{aboutUrl}'>درباره رهنمون</a></h3></header>" +
                               "<p>رهنمون با این هدف شکل گرفته است که به یاری مشاورانی متخصص به افراد کمک کند تا به آسانی پاسخی برای چالش های زندگی بیابند و از سویی بستری برای مشاوران فراهم آورد که با دسترسی به طیف گسترده ای از مراجعان کسب درآمد کنند.</p>" +
                               "</div>");
                       }
            };
        }

        public IEnumerable<HomeWidgetModel> GetHomeWidgets()
        {
            yield break;
        }
    }
}
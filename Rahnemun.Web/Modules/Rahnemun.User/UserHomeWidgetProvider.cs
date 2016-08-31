using System.Collections.Generic;
using System.Web;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.UI.Resources;
using Rahnemun.Common;
using Rahnemun.HomeContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UserHomeWidgetProvider : IHomeWidgetProvider
    {
        private readonly IContentLocator _contentLocator;

        public UserHomeWidgetProvider(IContentLocator contentLocator)
        {
            _contentLocator = contentLocator;
        }

        public IEnumerable<HomeSlideWidgetModel> GetHomeSlideWidgets()
        {
            yield break;
        }

        public IEnumerable<HomeWidgetModel> GetHomeIntroWidgets()
        {
            yield return new HomeWidgetModel
            {
                Priority = 800,
                Html = html =>
                       {
                           var joinUrl = html.GetUrlHelper().Route<IConsultantJoinUsRoute>().Get();
                           var joinImage = _contentLocator.GetContentUrl(GetType(), "~/Images/join-intro.jpg");
                           return new HtmlString(
                               $"<a title='استخدام به عنوان مشاور' href='{joinUrl}'><img src='{joinImage}' alt='استخدام به عنوان مشاور'></a>" +
                               "<div class='contents'>" +
                               $"<header><h3><a title='به رهنمون بپیوندید' href='{joinUrl}'>استخدام به عنوان مشاور</a></h3></header>" +
                               "<p>موسسه مشاوره آنلاین رهنمون قصد دارد با جذب تعداد محدودی از مشاوران متخصص، فعال و پرتوان، وب سایت خود را راه اندازی نماید. با پیوستن به تیم متخصصان رهنمون در کسب و کار آنلاین پیشتاز باشید و از همکاران خود پیشی بگیرید.</p>" +
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
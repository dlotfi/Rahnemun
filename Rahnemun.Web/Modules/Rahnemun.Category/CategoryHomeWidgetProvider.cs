using System.Collections.Generic;
using System.Web;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.UI.Resources;
using Rahnemun.CategoryContracts;
using Rahnemun.Common;
using Rahnemun.HomeContracts;

namespace Rahnemun.Category
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
                Priority = 900,
                Html = html =>
                       {
                           var categoriesListUrl = html.GetUrlHelper().Route<ICategoriesListRoute>().Get();
                           return new HtmlString(
                               "<h2>مشاوران متخصص</h2>" +
                               "<p>مشاور مناسب را از لیست مشاوران گروه مورد نظر خود انتخاب نموده و جلسه مشاوره را آغاز نمایید. پس از ثبت پرسش، در کمتر از 24 ساعت پاسخ خود را دریافت خواهید کرد.</p>" +
                               $"<a class='button default' title='گروه های مشاوره' href='{categoriesListUrl}'>گروه های مشاوره</a>");
                       },
                ImageUrl = url => _contentLocator.GetContentUrl(GetType(), "~/Images/category-slide.jpg")
            };
        }

        public IEnumerable<HomeWidgetModel> GetHomeIntroWidgets()
        {
            yield break;
        }

        public IEnumerable<HomeWidgetModel> GetHomeWidgets()
        {
            yield return new HomeWidgetModel
            {
                Priority = 1000,
                Html = html => html.WebPart<ICategoryListWebPart>().Get()
            };
        }
    }
}
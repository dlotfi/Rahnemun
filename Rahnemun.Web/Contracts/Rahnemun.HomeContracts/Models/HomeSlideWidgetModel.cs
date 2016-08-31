using System;
using System.Web.Mvc;

namespace Rahnemun.HomeContracts
{
    public class HomeSlideWidgetModel : HomeWidgetModel
    {
        public Func<UrlHelper, string> ImageUrl { get; set; }
        public Func<UrlHelper, string> MobileImageUrl { get; set; }
    }
}
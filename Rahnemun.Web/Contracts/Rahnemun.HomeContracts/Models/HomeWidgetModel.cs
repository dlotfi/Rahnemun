using System;
using System.Web;
using System.Web.Mvc;

namespace Rahnemun.HomeContracts
{
    public class HomeWidgetModel
    {
        public int Priority { get; set; }
        public Func<HtmlHelper, IHtmlString> Html { get; set; }
    }
}
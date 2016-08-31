using System;
using System.Web;
using System.Web.Mvc;

namespace Rahnemun.UserContracts
{
    public class DashboardItemModel
    {
        public string Title { get; set; }
        public int Priority { get; set; }
        public Func<HtmlHelper, IHtmlString> Html { get; set; }
    }
}
using System;
using System.Web;
using System.Web.Mvc;

namespace Rahnemun.UIContracts
{
    public class DialogModel
    {
        public string DialogId { get; set; }
        public Func<HtmlHelper, IHtmlString> Html { get; set; }
    }
}
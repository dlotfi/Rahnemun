using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;

namespace Rahnemun.UIContracts
{
    public static class Extensions
    {
        public static ISet<string> EmbededDialogs(this IWorkContext workContext)
        {
            return workContext.GetState<ISet<string>>("EmbededDialogs");
        }

        public static UIInfo GetUIInfo(this IViewDataContainer viewPage)
        {
            return viewPage.ViewData["_UIInfo"] as UIInfo;
        }

        public static void SetUIInfo(this IViewDataContainer viewPage, UIInfo uiInfo)
        {
            Throw.If(viewPage.ViewData.ContainsKey("_UIInfo"))
                .A<InvalidOperationException>("UIInfo has been set before.");
            viewPage.ViewData["_UIInfo"] = uiInfo;
        }
    }
}

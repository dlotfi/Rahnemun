using System;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc.ViewEngine;

namespace Rahnemun.Common
{
    public static class MetaDataExtensions
    {
        public static ContentInfo GetContentInfo(this IViewPage viewPage)
        {
            Throw.IfArgumentNull(viewPage, "viewPage");
            return viewPage.ViewData["_ContentInfo"] as ContentInfo;
        }

        public static void SetContentInfo(this IViewPage viewPage, ContentInfo contentInfo, bool setMetaData = true)
        {
            Throw.IfArgumentNull(viewPage, "viewPage");
            Throw.IfArgumentNull(contentInfo, "contentInfo");
            Throw.If(viewPage.ViewData.ContainsKey("_ContentInfo"))
                .A<InvalidOperationException>("ContentInfo has been set before");
            viewPage.ViewData["_ContentInfo"] = contentInfo;
            if (setMetaData)
            {
                var metaDataSetter = viewPage.Container.GetExportedValue<IMetaDataSetter>();
                var request = viewPage.WorkContext.CurrentHttpContext().Request;
                contentInfo.BaseUrl = "{0}://{1}".FormatWith(request.Url.Scheme, request.Headers["Host"]);
                metaDataSetter.SetMetaData(contentInfo);
            }
        }
    }
}

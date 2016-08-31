using System.Web.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.Common;
using Rahnemun.Common.ErrorHandling;
using Rahnemun.HomeContracts;

namespace Rahnemun.Home
{
    public class GeneralErrorHandler : IErrorHandler
    {
        public ActionResult GetErrorResult(ControllerContext controllerContext, ErrorType error, string message, string url)
        {
            if (error != ErrorType.General) return null;

            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var transferUrl = urlHelper.Route<IGeneralErrorRoute>().Get(new GeneralErrorRouteModel { Error = message });
            return new TransferResult(transferUrl);
        }
    }
}
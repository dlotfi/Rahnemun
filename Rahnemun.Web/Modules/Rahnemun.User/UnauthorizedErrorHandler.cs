using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Edreamer.Framework.UI.Notification;
using Rahnemun.Common;
using Rahnemun.Common.ErrorHandling;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UnauthorizedErrorHandler: IErrorHandler
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly INotifier _notifier;

        public UnauthorizedErrorHandler(IWorkContextAccessor workContextAccessor, INotifier notifier)
        {
            _workContextAccessor = workContextAccessor;
            _notifier = notifier;
        }

        public ActionResult GetErrorResult(ControllerContext controllerContext, ErrorType error, string message, string url)
        {
            if (error != ErrorType.Unauthorized) return null;

            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            if (_workContextAccessor.Context.CurrentUser() == null)
            {
                _notifier.Error("لطفا وارد رهنمون شوید. " + message);
                var redirectUrl = urlHelper.Route<ILogInRoute>().Get(url);
                return new RedirectResult(redirectUrl);
            }

            var transferUrl = urlHelper.Route<IUnauthorizedErrorRoute>().Get(new UnauthorizedErrorRouteModel { Error = message });
            return new TransferResult(transferUrl);
        }
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Mvc.Filters;
using Rahnemun.Common.ErrorHandling;

namespace Rahnemun.Common
{
    public class GlobalUnauthorizedFilter : FilterProviderBase, IActionFilter
    {
        private readonly IEnumerable<IErrorHandler> _errorHandlers;

        public GlobalUnauthorizedFilter(IEnumerable<IErrorHandler> errorHandlers)
        {
            _errorHandlers = errorHandlers;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //do nothing
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var unauthorizedResult = filterContext.Result as HttpUnauthorizedResult;
            if (unauthorizedResult == null) return;

            var message = unauthorizedResult.StatusDescription;
            var url = filterContext.HttpContext.Request.RawUrl;
            foreach (var errorHandler in _errorHandlers)
            {
                var result = errorHandler.GetErrorResult(filterContext, ErrorType.Unauthorized, message, url);
                if (result != null)
                {
                    filterContext.Result = result;
                    return;
                }
            }
        }
    }
}

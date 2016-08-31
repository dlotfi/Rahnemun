using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edreamer.Framework.Composition;
using Edreamer.Framework.Data.Infrastructure;
using Edreamer.Framework.Localization;
using Edreamer.Framework.Logging;
using Edreamer.Framework.Mvc.Filters;
using Edreamer.Framework.Security;
using Rahnemun.Common.ErrorHandling;

namespace Rahnemun.Common
{
    [PartPriority(PartPriorityAttribute.Default - 10)] // Run before SecurityFilter which transforms SecurityException to HttpUnauthorizedResult
                                                       // Remember that MVC executes exception filters in reverse order. 
    public class GlobalExceptionFilter : FilterProviderBase, IExceptionFilter
    {
        private readonly IEnumerable<IErrorHandler> _errorHandlers;

        public GlobalExceptionFilter(IEnumerable<IErrorHandler> errorHandlers)
        {
            _errorHandlers = errorHandlers;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;

            var message = GetDefaultErrorMessage(filterContext.Exception);

            Logger.Error(filterContext.Exception, message);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new FormattedJsonResult(new { Success = false, Message = message });
                filterContext.ExceptionHandled = true;    
            }
            else if (!(filterContext.Exception is SecurityException))  // If a SecurityException is occured, let the framework change it to HttpUnauthorizedResult
            {
                var url = filterContext.HttpContext.Request.RawUrl;
                foreach (var errorHandler in _errorHandlers)
                {
                    var result = errorHandler.GetErrorResult(filterContext, ErrorType.General, message, url);
                    if (result != null)
                    {
                        filterContext.Result = result;
                        filterContext.ExceptionHandled = true;
                    }
                }
            }
        }

        private string GetDefaultErrorMessage(Exception exception)
        {
            if (exception != null)
            {
                if (exception is SecurityException)
                    return (exception as SecurityException).LocalMessage ?? T("A security exception occurred.");
                if (exception is DataConcurrencyException)
                    return T("A Concurrency exception occured");
            }
            return T("An exception occured.");
        }
    }
}

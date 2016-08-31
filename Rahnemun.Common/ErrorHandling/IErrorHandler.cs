using System.Web.Mvc;
using Edreamer.Framework.Composition;

namespace Rahnemun.Common.ErrorHandling
{
    [InterfaceExport]
    public interface IErrorHandler
    {
        ActionResult GetErrorResult(ControllerContext controllerContext, ErrorType error, string message, string url);
    }
}
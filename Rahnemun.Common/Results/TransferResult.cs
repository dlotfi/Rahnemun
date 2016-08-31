using System.Web.Mvc;

namespace Rahnemun.Common
{
    /// <summary>
    /// Transfers execution to the supplied url.
    /// </summary>
    public class TransferResult : ActionResult
    {
        public string Url { get; private set; }

        public TransferResult(string url)
        {
            Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Server.TransferRequest(Url, true);
        }
    }
}
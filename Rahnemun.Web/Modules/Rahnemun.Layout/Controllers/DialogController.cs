using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.UI.Resources;
using Rahnemun.Common;
using Rahnemun.UIContracts;

namespace Rahnemun.Layout.Controllers
{
    public class DialogController : Controller
    {
        private readonly IEnumerable<IDialogProvider> _dialogProviders;
        private readonly IResourceManager _resourceManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public DialogController(IEnumerable<IDialogProvider> dialogProviders, IResourceManager resourceManager, IWorkContextAccessor workContextAccessor)
        {
            _dialogProviders = dialogProviders;
            _resourceManager = resourceManager;
            _workContextAccessor = workContextAccessor;
        }

        // GET(Ajax): /dialog
        [HttpGet, Ajax]
        public FormattedJsonResult GetDialog(string dialogId)
        {
            var dialog = _dialogProviders
                .SelectMany(p => p.GetDialogs())
                .FirstOrDefault(d => d.DialogId.EqualsIgnoreCase(dialogId));
            Throw.IfNull(dialog)
                .AnArgumentException("No dialog named '{0}' has been registered.".FormatWith(dialogId));

            var content = dialog.Html(this.HtmlHelper()).ToString();
            var resources = _resourceManager.GetAllRequiredResources()
                .Where(r => r.Resource.Type == "script" || r.Resource.Type == "stylesheet")
                .Select(r => r.GetHtml().ToString().Trim())
                .ToArray();
            return new FormattedJsonResult(new { Content = content, Resources = resources });
        }

        [ChildActionOnly]
        public PartialViewResult DialogsWebPart()
        {
            var embededDialogs = _workContextAccessor.Context.EmbededDialogs();
            var dialogs = _dialogProviders
                .SelectMany(p => p.GetDialogs())
                .Where(d => embededDialogs.Contains(d.DialogId))
                .ToList();
            return PartialView(dialogs);
        }
    }
}

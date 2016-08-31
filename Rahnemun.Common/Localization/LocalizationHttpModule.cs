using System;
using System.Globalization;
using System.Threading;
using System.Web;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Localization;
using Edreamer.Framework.Mvc.Extensions;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(Rahnemun.Common.LocalizationHttpModule), "Register")]
namespace Rahnemun.Common
{
    public class LocalizationHttpModule : IHttpModule
    {
        private static bool _isRegistered;

        public static void Register()
        {
            // All Register calls are made on the same thread, so no lock needed here.
            if (_isRegistered) return;

            DynamicModuleUtility.RegisterModule(typeof(LocalizationHttpModule));
            _isRegistered = true;
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        private void OnPreRequestHandlerExecute(object sender, EventArgs eventArgs)
        {
            var container = HttpContext.Current.Request.RequestContext.GetContainer();
            if (container != null) // (RequestContext.Route != null)
            {
                var workContextAccessor = container.GetExportedValue<IWorkContextAccessor>();
                var currentCultutre = workContextAccessor.Context.CurrentCulture();
                var culture = currentCultutre.EqualsIgnoreCase("fa-ir")
                    ? new PersianCulture()
                    : CultureInfo.GetCultureInfo(currentCultutre);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        public void Dispose()
        {
        }
    }
}
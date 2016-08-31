using Edreamer.Framework.UI.Resources;

namespace Rahnemun.JavaScript
{
    public class JavaScriptResourceRegistrar: IResourceRegistrar
    {
        public void RegisterResources(ResourceRegistrarContext context)
        {
            context.DefineScript("jQuery").SetUrl("Scripts/jquery-1.11.3.min.js").SetUrlDebug("Scripts/jquery-1.11.3.js").SetVersion("1.11.3");
            context.DefineScript("jQuery-Form").SetUrl("Scripts/jquery.form.min.js").SetUrlDebug("Scripts/jquery.form.js").SetVersion("3.51.0").SetDependencies("jQuery");
            context.DefineScript("jQuery-Validation-All").SetUrl("Scripts/jquery.validate.all.min.js").SetUrlDebug("Scripts/jquery.validate.all.js").SetDependencies("jQuery");
            //context.DefineScript("jQuery-Validation").SetUrl("Scripts/jquery.validate.min.js").SetUrlDebug("Scripts/jquery.validate.js").SetVersion("1.14.0").SetDependencies("jQuery");
            //context.DefineScript("jQuery-Validation-Unobtrusive").SetUrl("Scripts/jquery.validate.unobtrusive.min.js").SetUrlDebug("Scripts/jquery.validate.unobtrusive.js").SetVersion("3.2.3").SetDependencies("jQuery-Validation");
            //context.DefineScript("jQuery-Validation-AdditionalMethods").SetUrl("Scripts/jquery.validate-additionals.js").SetDependencies("jQuery-Validation");
            //context.DefineScript("jQuery-Validation-Extensions").SetUrl("Scripts/jquery.validate-extensions.js").SetDependencies("jQuery-Validation", "jQuery-Validation-Unobtrusive");
            context.DefineScript("jQuery-StickyKit").SetUrl("Scripts/jquery.sticky-kit.min.js").SetUrlDebug("Scripts/jquery.sticky-kit.js").SetVersion("1.1.2").SetDependencies("jQuery");
            context.DefineScript("Knockout").SetUrl("Scripts/knockout-3.3.0.js").SetUrlDebug("Scripts/knockout-3.3.0.debug.js").SetVersion("3.3.0");
            context.DefineScript("Rahnemun").SetUrl("Scripts/rahnemun.min.js").SetUrlDebug("Scripts/rahnemun.js").SetDependencies("jQuery");
        }
    }
}
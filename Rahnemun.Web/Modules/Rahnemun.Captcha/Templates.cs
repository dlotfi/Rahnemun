using System.Collections.Generic;
using Edreamer.Framework.Mvc.Templates;

namespace Rahnemun.Captcha
{
    public class CaptchaTemplateRegistrar : ITemplateRegistrar
    {
        public string TemplateContext
        {
            get { return "MainTemplate"; }
        }

        public string BaseTemplateContext
        {
            get { return null; }
        }

        public void RegisterTemplates(ICollection<Template> templates)
        {
            templates.Add(new Template { Name = "FullEditorTemplates/Captcha", Path = "FullEditorTemplates/Captcha" });
        }
    }
}
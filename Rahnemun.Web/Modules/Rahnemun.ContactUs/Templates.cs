using System.Collections.Generic;
using Edreamer.Framework.Mvc.Templates;

namespace Rahnemun.ContactUs
{
    public class MainTemplateRegistrar : ITemplateRegistrar
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
            templates.Add(new Template { Name = "EditorTemplates/CustomerMessageSubject", Path = "EditorTemplates/CustomerMessageSubject" });
        }
    }
}
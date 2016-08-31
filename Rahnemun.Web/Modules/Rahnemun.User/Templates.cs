using System.Collections.Generic;
using Edreamer.Framework.Mvc.Templates;

namespace Rahnemun.User
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
            templates.Add(new Template { Name = "FullEditorTemplates/Disclaimer", Path = "FullEditorTemplates/Disclaimer" });
            templates.Add(new Template { Name = "DisplayTemplates/EducationLevel", Path = "DisplayTemplates/EducationLevel" });
            templates.Add(new Template { Name = "DisplayTemplates/MaritalStatus", Path = "DisplayTemplates/MaritalStatus" });
            templates.Add(new Template { Name = "EditorTemplates/MaritalStatus", Path = "EditorTemplates/MaritalStatus" });
            templates.Add(new Template { Name = "EditorTemplates/EducationLevel", Path = "EditorTemplates/EducationLevel" });
            templates.Add(new Template { Name = "EditorTemplates/Gender", Path = "EditorTemplates/Gender" });
        }
    }
}
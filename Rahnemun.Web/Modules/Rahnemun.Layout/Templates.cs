using System.Collections.Generic;
using Edreamer.Framework.Mvc.Templates;

namespace Rahnemun.Layout
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
            templates.Add(new Template { Name = "FullDisplayTemplates/*", Path = "FullDisplayTemplates/Default" });
            templates.Add(new Template { Name = "FullEditorTemplates/*", Path = "FullEditorTemplates/Default" });
            templates.Add(new Template { Name = "FullEditorTemplates/Boolean", Path = "FullEditorTemplates/Boolean" });
            templates.Add(new Template { Name = "FullEditorTemplates/Upload", Path = "FullEditorTemplates/Upload" });
            templates.Add(new Template { Name = "FullEditorTemplates/CheckListBox", Path = "FullEditorTemplates/CheckListBox" });
            templates.Add(new Template { Name = "EditorTemplates/*", Path = "EditorTemplates/Text" });
            templates.Add(new Template { Name = "EditorTemplates/MultilineText", Path = "EditorTemplates/MultilineText" });
            templates.Add(new Template { Name = "EditorTemplates/SingleSelect", Path = "EditorTemplates/SingleSelect" });
            templates.Add(new Template { Name = "EditorTemplates/Password", Path = "EditorTemplates/Password" });
            templates.Add(new Template { Name = "EditorTemplates/DateTime", Path = "EditorTemplates/DateTime" });
            templates.Add(new Template { Name = "DisplayTemplates/Currency", Path = "DisplayTemplates/Currency" });
            templates.Add(new Template { Name = "DisplayTemplates/MultilineText", Path = "DisplayTemplates/MultilineText" });
        }
    }
}
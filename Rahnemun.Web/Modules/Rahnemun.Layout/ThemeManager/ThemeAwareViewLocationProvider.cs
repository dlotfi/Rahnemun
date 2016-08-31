using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Edreamer.Framework.Composition;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Module;
using Edreamer.Framework.Mvc.ViewEngine;
using Edreamer.Framework.Settings;

namespace Rahnemun.Layout.ThemeManager
{
    [PartPriority(PartPriorityAttribute.Default + 10)] // Overrides default ViewLocationProvider
    public class ThemeAwareViewLocationProvider: IViewLocationProvider
    {
        private readonly ICollection<string> _viewLocationFormats;
        private readonly ICollection<string> _layoutLocationFormats;
        private readonly ICollection<string> _templateLocationFormats;
        private readonly string _modulesPath;

        public ThemeAwareViewLocationProvider(ISettingsService settingsService) 
        {
            _viewLocationFormats = new List<string>();
            _layoutLocationFormats = new List<string>();
            _templateLocationFormats = new List<string>();

            _modulesPath = VirtualPathUtility.AppendTrailingSlash(PathHelpers.GetVirtualPath(settingsService.GetModulesPath()));

            var themeName = settingsService.GetThemeName();
            if (!String.IsNullOrEmpty(themeName))
            {
                _viewLocationFormats.Add(_modulesPath + "{2}/Views/{1}/" + themeName + "/{0}.cshtml");
                _viewLocationFormats.Add(_modulesPath + "{2}/Views/Shared/" + themeName + "/{0}.cshtml");
                _layoutLocationFormats.Add(_modulesPath + "{2}/Layouts/" + themeName + "/{0}.cshtml");
                _templateLocationFormats.Add(_modulesPath + "{2}/Templates/" + themeName + "/{0}.cshtml");
            }

            _viewLocationFormats.Add(_modulesPath + "{2}/Views/{1}/{0}.cshtml");
            _viewLocationFormats.Add(_modulesPath + "{2}/Views/Shared/{0}.cshtml");
            _layoutLocationFormats.Add(_modulesPath + "{2}/Layouts/{0}.cshtml");
            _templateLocationFormats.Add(_modulesPath + "{2}/Templates/{0}.cshtml");
        }

        public IEnumerable<string> AreaViewLocationFormats
        {
            get { return _viewLocationFormats; }
        }

        public IEnumerable<string> AreaMasterLocationFormats
        {
            get { return _viewLocationFormats; }
        }

        public IEnumerable<string> AreaPartialViewLocationFormats
        {
            get { return _viewLocationFormats; }
        }

        public IEnumerable<string> LayoutLocationFormats
        {
            get { return _layoutLocationFormats; }
        }

        public IEnumerable<string> TemplateLocationFormats
        {
            get { return _templateLocationFormats; }
        }

        public string GetBasePathFromVirtualPath(string viewPath)
        {
            Throw.IfArgumentNull(viewPath, "viewPath");
            Throw.IfNot(viewPath.StartsWith(_modulesPath))
                .AnArgumentException("This view is not defined in modules", "viewPath");
            var moduleRelativePath = viewPath.TrimStart(_modulesPath);
            var moduleName = moduleRelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).First();
            return VirtualPathUtility.Combine(_modulesPath, moduleName);
        }
    }
}
using System.Collections.Generic;
using System.Web;
using Edreamer.Framework.Composition;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Settings;
using Edreamer.Framework.UI.Resources;

namespace Rahnemun.Layout.ThemeManager
{
    [PartPriority(PartPriorityAttribute.Default + 100)]
    public class ThemeAwareResourceTagBuilder : ResourceTagBuilder
    {
        private readonly string _themeName;

        public ThemeAwareResourceTagBuilder(IWorkContextAccessor workContextAccessor, ISettingsService settingsService)
            : base(workContextAccessor)
        {
            Throw.IfArgumentNull(settingsService, "settingsService");
            _themeName = settingsService.GetThemeName();
        }

        public override IHtmlString BuildResourceTag(string type, string path, IDictionary<string, string> attributes, string condition, bool absoluteUrl)
        {
            path = path.Replace("{ThemeName}", _themeName);
            return base.BuildResourceTag(type, path, attributes, condition, absoluteUrl);
        }
    }
}
using System.Collections.Generic;
using Edreamer.Framework.Mvc.Layouts;

namespace Rahnemun.Layout
{
    public class LayoutRegistrar : ILayoutRegistrar
    {
        public void RegisterLayouts(ICollection<Edreamer.Framework.Mvc.Layouts.Layout> layouts)
        {
            layouts.Add(new Edreamer.Framework.Mvc.Layouts.Layout { Name = "MainLayout", Path = "MainLayout" });
        }
    }
}
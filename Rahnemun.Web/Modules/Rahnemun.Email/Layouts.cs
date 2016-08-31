using System.Collections.Generic;
using Edreamer.Framework.Mvc.Layouts;

namespace Rahnemun.Email
{
    public class LayoutRegistrar : ILayoutRegistrar
    {
        public void RegisterLayouts(ICollection<Layout> layouts)
        {
            layouts.Add(new Layout { Name = "EmailLayout", Path = "EmailLayout" });
        }
    }
}
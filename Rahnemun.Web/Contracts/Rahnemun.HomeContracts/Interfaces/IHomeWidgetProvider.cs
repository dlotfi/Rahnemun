using System.Collections.Generic;
using Edreamer.Framework.Composition;

namespace Rahnemun.HomeContracts
{
    [InterfaceExport]
    public interface IHomeWidgetProvider
    {
        IEnumerable<HomeSlideWidgetModel> GetHomeSlideWidgets();
        IEnumerable<HomeWidgetModel> GetHomeIntroWidgets();
        IEnumerable<HomeWidgetModel> GetHomeWidgets();
    }
}

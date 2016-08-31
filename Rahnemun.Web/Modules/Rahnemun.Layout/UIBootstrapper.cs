using System.Web;
using Edreamer.Framework.Bootstrapping;
using Edreamer.Framework.UI.Resources;
using Rahnemun.Common.MetaDataProviding.Providers;

namespace Rahnemun.Layout
{
    public class UIBootstrapper : IBootstrapperTask
    {
        private readonly string _defaultImagePath;

        public UIBootstrapper(IContentLocator contentLocator)
        {
            _defaultImagePath = VirtualPathUtility.ToAbsolute(contentLocator.GetContentUrl(GetType(), "~/Images/default.png"));
        }

        public void Run()
        {
            OpenGraphMetaDataProvider.DefaultImageUrl = _defaultImagePath;
        }
    }
}
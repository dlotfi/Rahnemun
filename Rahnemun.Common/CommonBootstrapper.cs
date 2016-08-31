using Edreamer.Framework.Bootstrapping;
using Edreamer.Framework.UI.Resources;
using Edreamer.Framework.Validation;

namespace Rahnemun.Common
{
    public class CommonBootstrapper : IBootstrapperTask
    {
        public void Run()
        {
            DataAnnotationsValidatorProvider.RegisterAdapterFactory(typeof(AcceptExtensionsAttribute),
                (metadata, attribute, localizer) => new AcceptExtensionsAttribute.Adapter(metadata, (AcceptExtensionsAttribute)attribute, localizer));
            ResourceTagBuilder.RegisterResourceTag("image", "img", "src", null, true);
        }
    }
}
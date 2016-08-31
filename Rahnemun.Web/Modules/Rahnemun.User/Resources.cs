using Edreamer.Framework.UI.Resources;

namespace Rahnemun.User
{
    public class UserResourceRegistrar : IResourceRegistrar
    {
        public void RegisterResources(ResourceRegistrarContext context)
        {
            context.DefineResource("image", "DefaultMaleProfilePicture").SetUrl("Images/avatar-m.jpg");
            context.DefineResource("image", "DefaultFemaleProfilePicture").SetUrl("Images/avatar-f.jpg");
            context.DefineResource("image", "DefaultUnknownProfilePicture").SetUrl("Images/avatar-u.jpg");
        }
    }
}
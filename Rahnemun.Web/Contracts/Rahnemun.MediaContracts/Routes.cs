using Edreamer.Framework.Mvc.Routes;

namespace Rahnemun.MediaContracts
{
    // For security reason (authorization access should be checked)
    //public interface IGetMediaRoute : INamedRoute<IdModel> { }

    public interface IImageRoute : INamedRoute<ImageRouteModel> { }
}
using Edreamer.Framework.Mvc.Routes;

namespace Rahnemun.BlogContracts
{
    public interface IBlogRoute : INamedRoute<BlogRouteModel> { }
    public interface IBlogPostRoute : INamedRoute<BlogPostRouteModel> { }
}

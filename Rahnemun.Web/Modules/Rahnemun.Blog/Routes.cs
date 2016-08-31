using System.Web.Mvc;
using Edreamer.Framework.Mvc.Routes;
using Rahnemun.BlogContracts;
using Rahnemun.Common;

namespace Rahnemun.Blog
{
    public class BlogRoute : NamedRoute<BlogRouteModel>, IBlogRoute { }
    public class BlogPostRoute : NamedRoute<BlogPostRouteModel>, IBlogPostRoute { }

    public class BlogRouteRegistrar : IRouteRegistrar
    {
        private readonly IBlogService _blogService;

        public BlogRouteRegistrar(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public void RegisterRoutes(RouteRegistrarContext context)
        {
            context.MapRoute<BlogRoute>("blog", new { Controller = "Blog", Action = "Index" });
            context.MapRoute<BlogPostRoute>("blog/{PostId}/{PostSlug}", new { Controller = "Blog", Action = "Post", PostSlug = UrlParameter.Optional },
                null, new BlogPostRouteCanonicalizer(_blogService, "PostId", "PostSlug"));
        }
    }
}
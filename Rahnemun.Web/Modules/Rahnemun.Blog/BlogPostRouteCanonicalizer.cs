using System.Globalization;
using System.Linq;
using System.Web.Routing;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc.Routes;
using Rahnemun.BlogContracts;

namespace Rahnemun.Blog
{
    public class BlogPostRouteCanonicalizer: IRouteCanonicalizer
    {
        private readonly IBlogService _blogService;
        private readonly string _idSegmentName;
        private readonly string _slugSegmentName;

        public BlogPostRouteCanonicalizer(IBlogService blogService, string idSegmentName, string slugSegmentName)
        {
            _blogService = blogService;
            _idSegmentName = idSegmentName;
            _slugSegmentName = slugSegmentName;
        }

        public RouteValueDictionary GetCanonicalRouteValues(RequestContext requestContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection != RouteDirection.IncomingRequest)
                return null;

            var valueProvider = new System.Web.ModelBinding.DictionaryValueProvider<object>(values, CultureInfo.InvariantCulture);
            var result = valueProvider.GetValue(_idSegmentName);
            var id = (int?)result?.ConvertTo(typeof(int));
            if (id == null)
                return null;
            var postSlug = _blogService.Posts
                .Where(p => p.Id == id)
                .Select(p => p.Slug)
                .SingleOrDefault();

            result = valueProvider.GetValue(_slugSegmentName);
            var slug = (string)result?.ConvertTo(typeof(string));
            if (postSlug == null && slug == null || postSlug.EqualsIgnoreCase(slug))
                return null;

            values[_slugSegmentName] = postSlug;
            return values;
        }
    }
}
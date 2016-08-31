using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Routing;
using Edreamer.Framework.Caching;
using Edreamer.Framework.Composition;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc.Routes;
using Rahnemun.CategoryContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class CategoryDetiailsRouteCanonicalizer: IRouteCanonicalizer
    {
        private readonly ICompositionContainerAccessor _compositionContainerAccessor;
        private readonly string _idSegmentName;
        private readonly string _slugSegmentName;
        private readonly ICache _cache;

        public CategoryDetiailsRouteCanonicalizer(ICompositionContainerAccessor compositionContainerAccessor, ICacheFactory cacheFactory, string idSegmentName, string slugSegmentName)
        {
            _cache = cacheFactory.CreateCache(GetType());
            _compositionContainerAccessor = compositionContainerAccessor;
            _idSegmentName = idSegmentName;
            _slugSegmentName = slugSegmentName;
        }

        private IDictionary<int, string> CategoriesSlugs
        {
            get
            {
                return _cache.Get("CategoriesSlugs", ctx => GetCategoriesSlugs());
            }
        }

        public RouteValueDictionary GetCanonicalRouteValues(RequestContext requestContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var valueProvider = new System.Web.ModelBinding.DictionaryValueProvider<object>(values, CultureInfo.InvariantCulture);
            var result = valueProvider.GetValue(_idSegmentName);
            var id = (int?)result?.ConvertTo(typeof(int));
            if (id == null)
                return null;

            var categorySlug = CategoriesSlugs[(int)id];
            
            result = valueProvider.GetValue(_slugSegmentName);
            var slug = (string)result?.ConvertTo(typeof(string));
            if (categorySlug == null && slug == null || categorySlug.EqualsIgnoreCase(slug))
                return null;

            values[_slugSegmentName] = categorySlug;
            return values;
        }

        private IDictionary<int, string> GetCategoriesSlugs()
        {
            var categoryService = _compositionContainerAccessor.Container.GetExportedValue<ICategoryService>();
            return categoryService.Categories
                .Select(c => new { Id = c.Id, Caption = c.CategoryGroup.Caption + " " + c.Caption })
                .ToList()
                .ToDictionary(cc => cc.Id, cc => cc.Caption?.Replace(" و ", "-").Replace(" ", "-"));
        }
    }

    public class ConsultantDisplayRouteCanonicalizer : IRouteCanonicalizer
    {
        private readonly IUserService _userService;
        private readonly string _idSegmentName;
        private readonly string _slugSegmentName;

        public ConsultantDisplayRouteCanonicalizer(IUserService userService, string idSegmentName, string slugSegmentName)
        {
            _userService = userService;
            _idSegmentName = idSegmentName;
            _slugSegmentName = slugSegmentName;
        }

        public RouteValueDictionary GetCanonicalRouteValues(RequestContext requestContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var valueProvider = new System.Web.ModelBinding.DictionaryValueProvider<object>(values, CultureInfo.InvariantCulture);
            var result = valueProvider.GetValue(_idSegmentName);
            var id = (int?)result?.ConvertTo(typeof(int));
            if (id == null)
                return null;

            var consultantName = _userService.Users
                .Where(u => u.Id == id)
                .Select(u => u.FirstName + " " + u.LastName)
                .SingleOrDefault();
            var consultantNameSlug = consultantName?.Replace(" ", "-");

            result = valueProvider.GetValue(_slugSegmentName);
            var slug = (string)result?.ConvertTo(typeof(string));
            if (consultantNameSlug == null && slug == null || consultantNameSlug.EqualsIgnoreCase(slug))
                return null;

            values[_slugSegmentName] = consultantNameSlug;
            return values;
        }
    }

}
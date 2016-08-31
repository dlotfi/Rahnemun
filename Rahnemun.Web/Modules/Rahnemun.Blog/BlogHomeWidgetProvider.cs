using System.Collections.Generic;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.BlogContracts;
using Rahnemun.HomeContracts;

namespace Rahnemun.Blog
{
    public class BlogHomeWidgetProvider : IHomeWidgetProvider
    {
        public IEnumerable<HomeSlideWidgetModel> GetHomeSlideWidgets()
        {
            yield break;
        }

        public IEnumerable<HomeWidgetModel> GetHomeIntroWidgets()
        {
            yield break;
        }

        public IEnumerable<HomeWidgetModel> GetHomeWidgets()
        {
            yield return new HomeWidgetModel
            {
                Priority = 800,
                Html = html => html.WebPart<IBlogNewPostsWebPart>().Get()
            };
        }
    }
}
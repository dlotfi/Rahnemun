using Rahnemun.BlogContracts;
using Edreamer.Framework.Mvc.WebParts;
using Rahnemun.Common;

namespace Rahnemun.Blog
{
    public class CommentsWebPart : ActionWebPart<IdModel>, ICommentsWebPart
    {
        public CommentsWebPart() : base("Comment", "CommentsWebPart") { }
    }

    public class BlogNewPostsWebPart : ActionWebPart, IBlogNewPostsWebPart
    {
        public BlogNewPostsWebPart() : base("Blog", "BlogNewPostsWebPart") { }
    }
}
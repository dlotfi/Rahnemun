using System;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Injection;
using Rahnemun.Blog.Models;
using Rahnemun.BlogContracts;
using Rahnemun.Common;
using Rahnemun.UserContracts;

namespace Rahnemun.Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IDateTimeLocalizationService _dateTimeLocalizationService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public BlogController(IBlogService blogService, IDateTimeLocalizationService dateTimeLocalizationService,
            IUserService userService, ICommentService commentService)
        {
            _blogService = blogService;
            _dateTimeLocalizationService = dateTimeLocalizationService;
            _userService = userService;
            _commentService = commentService;
        }

        // GET: /blog?Category=...&Tag=...
        [HttpGet]
        public ActionResult Index(string category, string tag)
        {
            var postsQuery = _blogService.Posts.Where(p => p.PublishTime <= DateTime.UtcNow);
            if (!String.IsNullOrEmpty(category))
            {
                category = category.ToLower();
                postsQuery = postsQuery.Where(p => p.Category.ToLower() == category);
            }
            if (!String.IsNullOrEmpty(tag))
            {
                tag = tag.ToLower();
                postsQuery = postsQuery.Where(p => p.Tags.ToLower().Contains(tag));
            }
            var posts = postsQuery.OrderByDescending(p => p.PublishTime).ToList()
                .Select(p => Injector.PlaneInject(new PostViewModel
                                                  {
                                                      PublishTime = p.PublishTime,
                                                      PublishTimeFormatted = p.PublishTime.ToLocalFormattedTime(_dateTimeLocalizationService),
                                                      AuthorFullName = _userService.GetUserFullName(p.User),
                                                      CommentsCount = _commentService.Comments.Count(c => c.BlogPost.Id == p.Id)
                                                  }, p));
            
            return View("Index", new IndexViewModel { Posts = posts, Category = category, Tag = tag });
        }

        // GET: /blog/{PostId}/{Slug}
        [HttpGet]
        public ActionResult Post(int postId)
        {
            var post = _blogService.GetPost(postId);
            
            if (post == null || post.PublishTime > DateTime.UtcNow) return HttpNotFound();

            var postViewModel = Injector.PlaneInject(new PostViewModel
                                                     {
                                                         PublishTime = post.PublishTime,
                                                         PublishTimeFormatted = post.PublishTime.ToLocalFormattedTime(_dateTimeLocalizationService),
                                                         AuthorFullName = _userService.GetUserFullName(post.User),
                                                         Author = post.User,
                                                         CommentsCount = _commentService.Comments.Count(c => c.BlogPost.Id == postId),
                                                         Tags = post.Tags.Split(';')
                                                     }, post);
            return View("Post", postViewModel);
        }

        [ChildActionOnly]
        public PartialViewResult SideBar(string category, string tag)
        {
            var categories = _blogService.Posts
                .Where(p => p.PublishTime <= DateTime.UtcNow)
                .Select(p => p.Category).Distinct()
                .Select(c => new BlogCategoryViewModel { Name = c, Selected = c.ToLower() == category });

            var tags = _blogService.Posts
                .Where(p => p.PublishTime <= DateTime.UtcNow)
                .Select(p => p.Tags).ToList()
                .SelectMany(ts => ts.Split(';'))
                .GroupBy(t => t)
                .Select(g => new BlogTagViewModel { Name = g.Key, Selected = g.Key.ToLower() == tag, PostsCount = g.Count() })
                .ToList();

            return PartialView("SideBar", new SidebarViewModel { Categories = categories, Tags = tags });
        }

        [ChildActionOnly]
        public ActionResult BlogNewPostsWebPart()
        {
            var posts = _blogService.Posts
                .Where(p => p.PublishTime <= DateTime.UtcNow)
                .OrderByDescending(p => p.PublishTime)
                .Take(3)
                .ToList()
                .Select(p => Injector.PlaneInject(new PostViewModel
                                                  {
                                                      PublishTime = p.PublishTime,
                                                      PublishTimeFormatted = p.PublishTime.ToLocalFormattedTime(_dateTimeLocalizationService),
                                                      AuthorFullName = _userService.GetUserFullName(p.User),
                                                      CommentsCount = _commentService.Comments.Count(c => c.BlogPost.Id == p.Id)
                                                  }, p));
            return PartialView(posts);
        }
    }
}

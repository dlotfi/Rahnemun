using System.Linq;
using Rahnemun.BlogContracts;
using Rahnemun.Domain;
using Rahnemun.UserContracts;

namespace Rahnemun.Blog.Services
{
    public class BlogService: IBlogService
    {
        private readonly IRahnemunDataContext _dataContext;

        public BlogService(IRahnemunDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<BlogPostModel> Posts
        {
            get
            {
                return _dataContext.BlogPosts.Select(p => new BlogPostModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    Summary = p.Summary,
                    Content = p.Content,
                    CallToAction = p.CallToAction,
                    PublishTime = p.PublishTime,
                    Category = p.Category,
                    Tags = p.Tags,
                    Slug = p.Slug,
                    CoverPictureId = p.CoverPictureId,
                    ThumbnailPictureId = p.ThumbnailPictureId,
                    User = new UserModel
                    {
                        Id = p.UserId,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Gender = p.User.Gender,
                        EducationLevel = p.User.EducationLevel,
                        MaritalStatus = p.User.MaritalStatus,
                        CellphoneNo = p.User.CellphoneNo,
                        BirthDate = p.User.BirthDate,
                        SubscribedToNewsletter = p.User.SubscribedToNewsletter,
                        More = p.User.More,
                        RegisterDate = p.User.RegisterDate,
                        ProfilePictureId = p.User.ProfilePictureId
                    }
                });
            }
        }


        public BlogPostModel GetPost(int id)
        {
            return Posts.SingleOrDefault(p => p.Id == id);
        }
    }
}
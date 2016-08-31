using System;
using System.Linq;
using Edreamer.Framework.Helpers;
using Rahnemun.BlogContracts;
using Rahnemun.Domain;
using Rahnemun.UserContracts;

namespace Rahnemun.Blog.Services
{
    public class CommentService: ICommentService
    {
        private readonly IRahnemunDataContext _dataContext;

        public CommentService(IRahnemunDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<CommentModel> Comments
        {
            get
            {
                return _dataContext.Comments.Select(c => new CommentModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    SentTime = c.SentTime,
                    RepliedCommentId = c.RepliedCommentId,
                    BlogPost = new BlogPostModel
                    {
                        Id = c.BlogPost.Id,
                        Title = c.BlogPost.Title,
                        Subtitle = c.BlogPost.Subtitle,
                        Summary = c.BlogPost.Summary,
                        Content = c.BlogPost.Content,
                        CallToAction = c.BlogPost.CallToAction,
                        PublishTime = c.BlogPost.PublishTime,
                        Category = c.BlogPost.Category,
                        Tags = c.BlogPost.Tags,
                        Slug = c.BlogPost.Slug,
                        CoverPictureId = c.BlogPost.CoverPictureId,
                        ThumbnailPictureId = c.BlogPost.ThumbnailPictureId,
                    },
                    User = c.User == null ? null : new UserModel
                    {
                        Id = c.User.Id,
                        FirstName = c.User.FirstName,
                        LastName = c.User.LastName,
                        Gender = c.User.Gender,
                        EducationLevel = c.User.EducationLevel,
                        MaritalStatus = c.User.MaritalStatus,
                        CellphoneNo = c.User.CellphoneNo,
                        BirthDate = c.User.BirthDate,
                        SubscribedToNewsletter = c.User.SubscribedToNewsletter,
                        More = c.User.More,
                        RegisterDate = c.User.RegisterDate,
                        ProfilePictureId = c.User.ProfilePictureId
                    },
                    Guest = c.Guest == null ? null : new GuestModel
                    {
                        Id = c.Guest.Id,
                        Name = c.Guest.Name,
                        Email = c.Guest.Email,
                        UserAgent = c.Guest.UserAgent,
                        UserIP = c.Guest.UserIP
                    }
                });
            }
        }

        public CommentModel GetComment(int id)
        {
            return Comments.SingleOrDefault(c => c.Id == id);
        }

        public CommentModel PostComment(int blogPostId, int? repliedCommentId, string text, int? userId, int? guestId)
        {
            Throw.If(userId == null && guestId == null || userId != null && guestId != null)
                .AnArgumentException("One of two parameters userId or guestId must be specified.");

            var commentEntity = new Comment
            {
                BlogPostId = blogPostId,
                Text = text,
                RepliedCommentId = repliedCommentId,
                UserId = userId,
                GuestId = guestId,
                SentTime = DateTime.UtcNow
            };
            _dataContext.Comments.Add(commentEntity);
            _dataContext.SaveChanges();
            return GetComment(commentEntity.Id);
        }
    }
}
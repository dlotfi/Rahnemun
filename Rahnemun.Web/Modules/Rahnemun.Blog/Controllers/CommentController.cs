using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Mvc;
using Edreamer.Framework.Mvc.Extensions;
using Edreamer.Framework.Security;
using Rahnemun.Blog.Models;
using Rahnemun.BlogContracts;
using Rahnemun.Common;
using Rahnemun.MediaContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly IDateTimeLocalizationService _dateTimeLocalizationService;
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IConsultantService _consultantService;

        public CommentController(IBlogService blogService, ICommentService commentService, IUserService userService,
            IGuestService guestService, IConsultantService consultantService, IWorkContextAccessor workContextAccessor,
            IDateTimeLocalizationService dateTimeLocalizationService)
        {
            _blogService = blogService;
            _commentService = commentService;
            _userService = userService;
            _guestService = guestService;
            _consultantService = consultantService;
            _workContextAccessor = workContextAccessor;
            _dateTimeLocalizationService = dateTimeLocalizationService;
        }

        [ChildActionOnly]
        public PartialViewResult CommentsWebPart(int id)
        {
            var comments = _commentService.Comments
                .Where(c => c.BlogPost.Id == id)
                .ToList();
            var commentsViewModels = comments
                .Where(c => c.RepliedCommentId == null) // Top-level comments
                .Select(c => CreateCommentViewModel(c, comments))
                .ToList();
            var currentUser = _workContextAccessor.Context.CurrentUser();
            var isUserLoggedin = currentUser != null;
            var guest = isUserLoggedin ? null : _guestService.GetCurrentGuest();
            return PartialView("CommentsWebPart", new CommentsWebPartViewModel
                                                  {
                                                      Comments = commentsViewModels,
                                                      IsUserLoggedin = isUserLoggedin,
                                                      Email = guest?.Email,
                                                      Name = guest?.Name,
                                                      PostCommentUrl = Url.Action("PostComment", new { Id = id }),
            });
        }

        // POST(Ajax): /blog/{Id}/post-comment
        [Ajax, HttpPost]
        public FormattedJsonResult PostComment(int id, int? repliedCommentId, CommentEditViewModel commentViewModel)
        {
            Throw.IfNot(_blogService.Posts.Any(p => p.Id == id))
                .AnArgumentException("No post with id {0} exists.".FormatWith(id), "id");

            var currentUser = _workContextAccessor.Context.CurrentUser();
            var isUserLoggedin = currentUser != null;
            if (isUserLoggedin)
            {
                ModelState["Name"].Errors.Clear();
                ModelState["Email"].Errors.Clear();
            }
            
            if (ModelState.IsValid)
            {
                var guest = isUserLoggedin ? null : _guestService.SetCurrentGuest(commentViewModel.Email, commentViewModel.Name, Request.ServerVariables);
                var commentModel = _commentService.PostComment(id, repliedCommentId, commentViewModel.Text, currentUser?.Id, guest?.Id);
                var comment = new CommentItem
                              {
                                  Id = commentModel.Id,
                                  Text = commentModel.Text,
                                  SentTime = commentModel.SentTime.ToIsoTime(),
                                  SentTimeFormatted = commentModel.SentTime.ToLocalFormattedTime(_dateTimeLocalizationService)
                              };
                if (isUserLoggedin)
                {
                    var authorName = _userService.GetUserFullName(commentModel.User);
                    comment.AuthorName = authorName;
                    comment.AuthorUrl = _consultantService.IsConsultant(commentModel.User.Id)
                        ? Url.Route<IConsultantDisplayRoute>().Get(new ConsultantIdModel {ConsultantId = commentModel.User.Id})
                        : null;
                    comment.AuthorProfilePictureHtml = this.HtmlHelper()
                        .ProfilePicture(commentModel.User.ProfilePictureId, commentModel.User.Gender, authorName, ImageSize.Comment).ToString();
                }
                else
                {
                    comment.AuthorName = guest.Name;
                    comment.AuthorProfilePictureHtml = this.HtmlHelper()
                        .ProfilePicture(null, null, guest.Name, ImageSize.Comment).ToString();
                }
                return new FormattedJsonResult(new { Success = true, IsUserLoggedin = isUserLoggedin, Comment = comment });
            }

            return new FormattedJsonResult(new { success = false, errors = ModelState.Errors(), isUserLoggedin }, false);
        }

        private CommentViewModel CreateCommentViewModel(CommentModel comment, IEnumerable<CommentModel> comments)
        {
            var commentViewModel = new CommentViewModel
                                   {
                                       Id = comment.Id,
                                       Text = comment.Text,
                                       SentTime = comment.SentTime.ToIsoTime(),
                                       SentTimeFormatted = comment.SentTime.ToLocalFormattedTime(_dateTimeLocalizationService)
                                   };

            if (comment.User != null)
            {
                commentViewModel.AuthorName = _userService.GetUserFullName(comment.User);
                if (_consultantService.IsConsultant(comment.Id))
                    commentViewModel.AuthorUrl = Url.Route<IConsultantDisplayRoute>().Get(new ConsultantIdModel { ConsultantId = comment.Id });
                commentViewModel.AuthorProfilePictureId = comment.User.ProfilePictureId;
                commentViewModel.AuthorGender = comment.User.Gender;
            }
            else // comment.Guest != null
            {
                commentViewModel.AuthorName = comment.Guest.Name;
            }

            commentViewModel.Replies = comments
                .Where(c => c.RepliedCommentId == comment.Id)
                .Select(c => CreateCommentViewModel(c, comments))
                .ToList();

            return commentViewModel;
        }

        private class CommentItem
        {
            public int Id { get; set; }
            public string AuthorName { get; set; }
            public string AuthorUrl { get; set; }
            public string AuthorProfilePictureHtml { get; set; }
            public string SentTimeFormatted { get; set; }
            public string SentTime { get; set; }
            public string Text { get; set; }
        }
    }
}

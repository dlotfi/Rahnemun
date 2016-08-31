using System.Collections.Generic;

namespace Rahnemun.Blog.Models
{
    public class CommentsWebPartViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public string PostCommentUrl { get; set; }
        public bool IsUserLoggedin { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
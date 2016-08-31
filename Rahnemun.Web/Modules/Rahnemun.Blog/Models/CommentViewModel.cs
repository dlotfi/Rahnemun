using System.Collections.Generic;
using Rahnemun.Common;

namespace Rahnemun.Blog.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public string SentTime { get; set; }
        public string SentTimeFormatted { get; set; }
        public string Text { get; set; }
        public int? AuthorProfilePictureId { get; set; }
        public Gender? AuthorGender { get; set; }
        public IEnumerable<CommentViewModel> Replies { get; set; }
    }
}
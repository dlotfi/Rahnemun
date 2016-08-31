using System;
using Rahnemun.UserContracts;

namespace Rahnemun.BlogContracts
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime SentTime { get; set; }

        public BlogPostModel BlogPost { get; set; }

        public UserModel User { get; set; }
        
        public GuestModel Guest { get; set; }

        public int? RepliedCommentId { get; set; }
    }
}
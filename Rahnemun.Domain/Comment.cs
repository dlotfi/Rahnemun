using System;
using System.ComponentModel.DataAnnotations;

namespace Rahnemun.Domain
{
    public class Comment
    {
        public int Id { get; set; }

        [Required, MaxLength(2000)]
        public string Text { get; set; }
        public DateTime SentTime { get; set; }

        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? GuestId { get; set; }
        public Guest Guest { get; set; }

        public int? RepliedCommentId { get; set; }
        public Comment RepliedComment { get; set; }
    }
}

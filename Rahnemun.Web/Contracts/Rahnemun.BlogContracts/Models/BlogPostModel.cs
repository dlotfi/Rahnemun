using System;
using Rahnemun.UserContracts;

namespace Rahnemun.BlogContracts
{
    public class BlogPostModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string CallToAction { get; set; }
        public DateTime PublishTime { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public string Slug { get; set; }
        public int CoverPictureId { get; set; }
        public int ThumbnailPictureId { get; set; }
        public UserModel User { get; set; }
    }
}
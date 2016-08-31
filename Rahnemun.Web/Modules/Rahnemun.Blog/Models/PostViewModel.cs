using System;
using System.Collections.Generic;
using Rahnemun.UserContracts;

namespace Rahnemun.Blog.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string CallToAction { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Slug { get; set; }
        public DateTime PublishTime { get; set; }
        public string PublishTimeFormatted { get; set; }
        public int ThumbnailPictureId { get; set; }
        public int CoverPictureId { get; set; }
        public string AuthorFullName { get; set; }
        public UserModel Author { get; set; }
        public int CommentsCount { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using Edreamer.Framework.DataAnnotations;

namespace Rahnemun.Domain
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required, ShortString]
        public string Title { get; set; }
        [ShortString]
        public string Subtitle { get; set; }
        [Required, MaxLength(1000)]
        public string Summary { get; set; }
        [Required]
        public string Content { get; set; }
        public string CallToAction { get; set; }
        public DateTime PublishTime { get; set; }
        [Required, ShortString]
        public string Category { get; set; }
        [MaxLength(512)]
        public string Tags { get; set; }
        [Required, MaxLength(100)]
        public string Slug { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CoverPictureId { get; set; }
        public int ThumbnailPictureId { get; set; }
    }
}
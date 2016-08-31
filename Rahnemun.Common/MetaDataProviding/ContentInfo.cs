using System;
using System.Collections.Generic;

namespace Rahnemun.Common
{
    public class ContentInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public string BaseUrl { get; set; }
    }

    public class ArticleContentInfo : ContentInfo
    {
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public Gender? AuthorGender { get; set; }
        public DateTime PublishTime { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }


    public class ProfileContentInfo : ContentInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
    }
}
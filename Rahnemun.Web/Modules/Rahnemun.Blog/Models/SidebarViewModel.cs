using System.Collections.Generic;

namespace Rahnemun.Blog.Models
{
    public class SidebarViewModel
    {
        public IEnumerable<BlogCategoryViewModel> Categories { get; set; }
        public IEnumerable<BlogTagViewModel> Tags { get; set; }
    }

    public class BlogCategoryViewModel
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class BlogTagViewModel
    {
        public string Name { get; set; }
        public int PostsCount { get; set; }
        public bool Selected { get; set; }
    }
}
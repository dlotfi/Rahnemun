using System.Collections.Generic;

namespace Rahnemun.Blog.Models
{
    public class IndexViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
    }
}
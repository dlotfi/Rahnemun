using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.BlogContracts
{
    [InterfaceExport]
    public interface IBlogService
    {
        IQueryable<BlogPostModel> Posts { get; }
        BlogPostModel GetPost(int id);
    }
}
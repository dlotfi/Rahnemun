using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.BlogContracts
{
    [InterfaceExport]
    public interface ICommentService
    {
        IQueryable<CommentModel> Comments { get; }
        CommentModel GetComment(int id);

        CommentModel PostComment(int blogPostId, int? repliedCommentId, string text, int? userId, int? guestId);
    }
}
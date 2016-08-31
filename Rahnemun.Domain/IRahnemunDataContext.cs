using Edreamer.Framework.Data;

namespace Rahnemun.Domain
{
    public interface IRahnemunDataContext : IDataContext
    {
        IRepository<BlogPost> BlogPosts { get; }
        IRepository<Category> Categories { get; }
        IRepository<CategoryGroup> CategoryGroups { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Consultant> Consultants { get; }
        IRepository<CustomerMessage> CustomerMessages { get; }
        IRepository<Guest> Guests { get; }
        IRepository<Message> Messages { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Session> Sessions { get; }
        IRepository<User> Users { get; }
    }
}

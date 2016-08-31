using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IUserService
    {
        IQueryable<UserModel> Users { get; }
        UserModel GetUser(int id);

        void AddUser(UserUpdateModel user);
        void UpdateUser(UserUpdateModel user);
        void DeleteUser(int id, byte[] timestamp);

        string GetUserFullName(UserModel user, bool withTitle = false);
    }
}

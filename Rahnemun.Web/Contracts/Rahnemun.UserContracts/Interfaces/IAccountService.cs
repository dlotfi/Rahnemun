using Edreamer.Framework.Composition;
using Edreamer.Framework.Security;
using Rahnemun.EmailContracts;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IAccountService
    {
        bool SendChallengeEmail(string usernameOrEmail, EmailCreatorFunc emailCreator, bool forPasswordReset);
        User ValidateChallenge(string nonce);
    }

    public delegate EmailModel EmailCreatorFunc(User user, string nonce);
}

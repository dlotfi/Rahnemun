using System.Collections.Generic;
using Edreamer.Framework.Mvc.Extensions;
using Rahnemun.UIContracts;
using Rahnemun.UserContracts;

namespace Rahnemun.User
{
    public class UserDialogProvider: IDialogProvider
    {
        public IEnumerable<DialogModel> GetDialogs()
        {
            yield return new DialogModel
                         {
                             DialogId = "login-dialog",
                             Html = html => html.WebPart<ILoginDialogWebPart>().Get()
                         };
        }
    }
}
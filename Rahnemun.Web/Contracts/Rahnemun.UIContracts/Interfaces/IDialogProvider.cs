using System.Collections.Generic;
using Edreamer.Framework.Composition;

namespace Rahnemun.UIContracts
{
    [InterfaceExport]
    public interface IDialogProvider
    {
        IEnumerable<DialogModel> GetDialogs();
    }
}
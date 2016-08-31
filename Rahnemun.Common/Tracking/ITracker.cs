using System.ComponentModel.Composition;
using Edreamer.Framework.Composition;

namespace Rahnemun.Common
{
    [InterfaceExport(Scope.Request, CreationPolicy.Shared)]
    public interface ITracker
    {
        void AddDestination(string destinationPath);

        void AddTransaction(string transactionId, string productId, string productName, string productCategory, decimal productPrice);

        string GetTrackingCode();
    }
}
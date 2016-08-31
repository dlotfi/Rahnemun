using System.Collections.Generic;
using Edreamer.Framework.Composition;

namespace Rahnemun.Common
{
    [InterfaceExport]
    public interface IMetaDataProvider
    {
        IEnumerable<MetaData> GetMetaData(ContentInfo contentInfo);
    }
}
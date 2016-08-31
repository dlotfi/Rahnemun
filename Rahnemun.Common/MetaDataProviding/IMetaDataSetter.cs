using Edreamer.Framework.Composition;

namespace Rahnemun.Common
{
    [InterfaceExport]
    public interface IMetaDataSetter
    {
        void SetMetaData(ContentInfo contentInfo);
    }
}
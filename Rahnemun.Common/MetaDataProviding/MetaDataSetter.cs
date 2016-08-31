using System.Collections.Generic;
using System.Linq;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.UI.MetaData;

namespace Rahnemun.Common
{
    public class MetaDataSetter: IMetaDataSetter
    {
        private readonly IEnumerable<IMetaDataProvider> _providers;
        private readonly IMetaDataManager _metaDataManager;

        public MetaDataSetter(IEnumerable<IMetaDataProvider> providers, IMetaDataManager metaDataManager)
        {
            _providers = providers;
            _metaDataManager = metaDataManager;
        }


        public void SetMetaData(ContentInfo contentInfo)
        {
            Throw.IfArgumentNull(contentInfo, "contentInfo");
            var metaData = _providers.SelectMany(p => p.GetMetaData(contentInfo)).ToList();
            foreach (var meta in metaData)
            {
                var metaEntry = new MetaEntry { Content = meta.Value };
                if (meta.Type == MetaDataType.Normal)
                    metaEntry.Name = meta.Name;
                else // meta.Type == MetaDataType.RDFa
                    metaEntry.AddAttribute("property", meta.Name);
                _metaDataManager.IncludeMetaData(metaEntry);
            }
        }


    }
}
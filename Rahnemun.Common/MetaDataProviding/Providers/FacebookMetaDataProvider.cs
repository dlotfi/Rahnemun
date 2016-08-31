using System;
using System.Collections.Generic;
using Edreamer.Framework.Settings;

namespace Rahnemun.Common.MetaDataProviding.Providers
{
    public class FacebookMetaDataProvider: IMetaDataProvider
    {
        private readonly ISettingsService _settingsService;

        public FacebookMetaDataProvider(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IEnumerable<MetaData> GetMetaData(ContentInfo contentInfo)
        {
            string appId;
            IEnumerable<string> admins;
            string page;
            _settingsService.TryGetSetting(new SettingEntryKey { Category = "Facebook", Name = "AppId" }, out appId);
            _settingsService.TryGetSetting(new SettingEntryKey { Category = "Facebook", Name = "Admins" }, out admins);
            _settingsService.TryGetSetting(new SettingEntryKey { Category = "Facebook", Name = "Page" }, out page);

            var metaDataList = new List<MetaData>();
            AddMetaData(metaDataList, "fb:app_id", appId);
            foreach (var admin in admins)
                AddMetaData(metaDataList, "fb:admins", admin);

            // Article
            var articleContentInfo = contentInfo as ArticleContentInfo;
            if (articleContentInfo != null)
                AddMetaData(metaDataList, "article:publisher", page);

            return metaDataList;
        }

        private static void AddMetaData(IList<MetaData> metaDataList, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
                metaDataList.Add(new MetaData { Name = name, Value = value, Type = MetaDataType.RDFa });
        }
    }
}
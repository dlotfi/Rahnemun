using System;
using System.Collections.Generic;
using System.Linq;
using Edreamer.Framework.Settings;

namespace Rahnemun.Common.MetaDataProviding.Providers
{
    public class TwitterMetaDataProvider : IMetaDataProvider
    {
        public static string DefaultImageUrl { get; set; }

        private readonly ISettingsService _settingsService;

        public TwitterMetaDataProvider(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IEnumerable<MetaData> GetMetaData(ContentInfo contentInfo)
        {
            string username;
            if (!_settingsService.TryGetSetting(new SettingEntryKey {Category = "Twitter", Name = "Username"}, out username))
                return Enumerable.Empty<MetaData>();
            username = "@" + username.TrimStart('@'); // Ensures @ at the start of twitter username 
            
            var metaDataList = new List<MetaData>();
            AddMetaData(metaDataList, "twitter:card", "summary");
            AddMetaData(metaDataList, "twitter:title", contentInfo.Title);
            AddMetaData(metaDataList, "twitter:description", contentInfo.Description);
            AddMetaData(metaDataList, "twitter:image", contentInfo.ImageUrl ?? DefaultImageUrl, contentInfo.BaseUrl);
            AddMetaData(metaDataList, "twitter:site", username);

            return metaDataList;
        }

        private static void AddMetaData(IList<MetaData> metaDataList, string name, string value, string baseUrl = null)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (!String.IsNullOrEmpty(baseUrl) && !Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    value = baseUrl + value;
                metaDataList.Add(new MetaData { Name = name, Value = value, Type = MetaDataType.Normal });
            }
        }
    }
}
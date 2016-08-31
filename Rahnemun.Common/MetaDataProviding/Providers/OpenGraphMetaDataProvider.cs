using System;
using System.Collections.Generic;
using System.Web;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Localization;

namespace Rahnemun.Common.MetaDataProviding.Providers
{
    public class OpenGraphMetaDataProvider: IMetaDataProvider
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        public static string DefaultImageUrl { get; set; }

        public OpenGraphMetaDataProvider(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public IEnumerable<MetaData> GetMetaData(ContentInfo contentInfo)
        {
            var metaDataList = new List<MetaData>();
            AddMetaData(metaDataList, "og:title", contentInfo.Title);
            AddMetaData(metaDataList, "og:description", contentInfo.Description);
            AddMetaData(metaDataList, "og:image", contentInfo.ImageUrl ?? DefaultImageUrl, contentInfo.BaseUrl);
            AddMetaData(metaDataList, "og:url", contentInfo.Url, contentInfo.BaseUrl);
            AddMetaData(metaDataList, "og:site_name", "رهنمون");
            AddMetaData(metaDataList, "og:locale", FixLocaleName(_workContextAccessor.Context.CurrentCulture()));
            AddMetaData(metaDataList, "og:type", GetTypeName(contentInfo));

            // Article
            var articleContentInfo = contentInfo as ArticleContentInfo;
            if (articleContentInfo != null)
            {
                
                AddMetaData(metaDataList, "article:author:first_name", articleContentInfo.AuthorFirstName);
                AddMetaData(metaDataList, "article:author:last_name", articleContentInfo.AuthorLastName);
                AddMetaData(metaDataList, "article:author:gender", GetGenderName(articleContentInfo.AuthorGender));
                AddMetaData(metaDataList, "article:published_time", articleContentInfo.PublishTime.ToIsoTime());
                AddMetaData(metaDataList, "article:section", articleContentInfo.Category);
                foreach (var tag in CollectionHelpers.EmptyIfNull(articleContentInfo.Tags))
                    AddMetaData(metaDataList, "article:tag", tag);
                return metaDataList;
            }

            // Profile
            var profileContentInfo = contentInfo as ProfileContentInfo;
            if (profileContentInfo != null)
            {
                AddMetaData(metaDataList, "profile:first_name", profileContentInfo.FirstName);
                AddMetaData(metaDataList, "profile:last_name", profileContentInfo.LastName);
                AddMetaData(metaDataList, "profile:gender", GetGenderName(profileContentInfo.Gender));
                return metaDataList;
            }

            return metaDataList;
        }

        private static void AddMetaData(IList<MetaData> metaDataList, string name, string value, string baseUrl = null)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (!String.IsNullOrEmpty(baseUrl) && !Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    value = baseUrl + value;
                metaDataList.Add(new MetaData { Name = name, Value = value, Type = MetaDataType.RDFa });
            }
        }

        private static string EnsureAbsoluteUrl(string baseUrl, string url)
        {
            if (!String.IsNullOrEmpty(url) && !url.StartsWith(baseUrl))
                return baseUrl + url;
            return url;
        }

        private static string GetTypeName(ContentInfo contentInfo)
        {
            if (contentInfo is ArticleContentInfo)
                return "article";
            if (contentInfo is ProfileContentInfo)
                return "profile";
            return "website";
        }

        private static string GetGenderName(Gender? gender)
        {
            if (gender == null) return null;

            switch (gender)
            {
                case Gender.Male: return "male";
                case Gender.Female: return "female";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        private static string FixLocaleName(string locale)
        {
            return locale?.Replace('-', '_');
        }
    }
}
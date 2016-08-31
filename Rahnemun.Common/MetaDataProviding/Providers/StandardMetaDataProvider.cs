using System;
using System.Collections.Generic;

namespace Rahnemun.Common.MetaDataProviding.Providers
{
    public class StandardMetaDataProvider : IMetaDataProvider
    {
        public IEnumerable<MetaData> GetMetaData(ContentInfo contentInfo)
        {
            var metaDataList = new List<MetaData>();
            AddMetaData(metaDataList, "description", contentInfo.Description);
            
            // Article
            var articleContentInfo = contentInfo as ArticleContentInfo;
            if (articleContentInfo != null)
            {
                AddMetaData(metaDataList, "author", GetAuthorName(articleContentInfo.AuthorFirstName, articleContentInfo.AuthorLastName));
            }

            return metaDataList;
        }

        private static void AddMetaData(IList<MetaData> metaDataList, string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
                metaDataList.Add(new MetaData { Name = name, Value = value, Type = MetaDataType.Normal });
        }

        private static string GetAuthorName(string firstName, string lastName)
        {
            var name = "";
            if (!String.IsNullOrEmpty(firstName))
                name += firstName;
            if (!String.IsNullOrEmpty(lastName))
                name += (name == "" ? "" : " ") + lastName;
            return name;
        }
    }
}
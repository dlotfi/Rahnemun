namespace Rahnemun.Common
{
    public class MetaData
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public MetaDataType Type { get; set; }
    }

    public enum MetaDataType
    {
        Normal,
        RDFa
    }
}
namespace Rahnemun.MediaContracts
{
    public class ImageWebPartModel : ImageRouteModel
    {
        public string Description { get; set; }
        public string DefaultImageResourceName { get; set; }
        public bool IncludeSize { get; set; }
    }
}

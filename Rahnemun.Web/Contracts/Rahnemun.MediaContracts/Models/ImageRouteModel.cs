namespace Rahnemun.MediaContracts
{
    public class ImageRouteModel
    {
        public int? Id { get; set; }
        public ImageSize? Size { get; set; }
        public bool MaxFit { get; set; }
        public string DefaultImagePath { get; set; }
    }
}
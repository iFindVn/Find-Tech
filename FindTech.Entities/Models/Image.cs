using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Image : Entity
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public ImageType ImageType { get; set; }
    }
}

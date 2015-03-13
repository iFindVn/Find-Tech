using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Video : Entity
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
    }
}

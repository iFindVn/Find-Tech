using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Models
{
    public class LikeViewModel
    {
        public int LikeId { get; set; }
        public string WhoLikeEmail { get; set; }
        public int ObjectId { get; set; }
        public ObjectType ObjectType { get; set; }
    }
}
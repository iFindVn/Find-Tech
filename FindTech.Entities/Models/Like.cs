using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Like : Entity
    {
        public int LikeId { get; set; }
        public string WhoLikeEmail { get; set; }
        public int ObjectId { get; set; }
        public ObjectType ObjectType { get; set; }
    }
}

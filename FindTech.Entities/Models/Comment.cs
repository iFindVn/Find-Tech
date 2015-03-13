using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Comment : Entity
    {
        public int CommentId { get; set; }
        public string CommentatorEmail { get; set; }
        public string Content { get; set; }
        public int ObjectId { get; set; }
        public ObjectType ObjectType { get; set; }
    }
}

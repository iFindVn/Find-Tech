using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Opinion : Entity
    {
        public int OpinionId { get; set; }
        public int OpinionCount { get; set; }
        public OpinionLevel OpinionLevel { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}

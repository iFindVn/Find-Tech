using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class RatingScore : Entity
    {
        public int RatingScoreId { get; set; }
        public int Score { get; set; }
        public int RatingId { get; set; }
        public virtual Rating Rating { get; set; }
        public int RatingCriteriaId { get; set; }
        public virtual RatingCriteria RatingCriteria { get; set; }
    }
}

using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class RatingCriteria : Entity
    {
        public int RatingCriteriaId { get; set; }
        public string RatingCriteriaName { get; set; }
    }
}

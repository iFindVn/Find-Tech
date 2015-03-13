using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Rating : Entity
    {
        public int RatingId { get; set; }
        public string Email { get; set; }
        public string Opinion { get; set; }
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
    }
}

using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Models
{
    public class OpinionViewModel
    {
        public int OpinionId { get; set; }
        public int OpinionCount { get; set; }
        public string OpinionText { get; set; }
        public string OpinionBackground { get; set; }
        public OpinionLevel OpinionLevel { get; set; }
    }
}
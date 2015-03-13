using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Xpath : Entity
    {
        public int XpathId { get; set; }
        public string XpathString { get; set; }
        public ArticleField ArticleField { get; set; }
        public int SourceId { get; set; }
        public virtual Source Source { get; set; }
    }
}

using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Source : Entity
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public virtual ICollection<Xpath> Xpaths { get; set; } 
        public virtual ICollection<Article> Articles { get; set; } 
    }
}

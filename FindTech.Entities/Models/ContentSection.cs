using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class ContentSection : Entity
    {
        public ContentSection()
        {
            Images = new List<Image>();
        }
        public int ContentSectionId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionDescription { get; set; }
        public string SectionContent { get; set; }
        public int PageNumber { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public int BenchmarkGroupId { get; set; }
        public virtual ICollection<Image> Images { get; set; } 
    }
}

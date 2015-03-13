using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;

namespace FindTech.Entities.StoredProcedures.Models
{
    public class SearchArticlesResult
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public int Priority { get; set; }
        public string Avatar { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public BoxSize BoxSize { get; set; }
        public bool? IsActived { get; set; }
        public bool? IsHot { get; set; }
        public string ArticleCategoryColor { get; set; }
        public string ArticleCategoryName { get; set; }
        public string ArticleCategorySeoName { get; set; }
        public string CreatedUserDisplayName { get; set; }
        public string SourceName { get; set; }
        public string SourceLogo { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
    }
}

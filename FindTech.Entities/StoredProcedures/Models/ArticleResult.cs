using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;

namespace FindTech.Entities.StoredProcedures.Models
{
    public class ArticleResult
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public int Priority { get; set; }
        public string Avatar { get; set; }
        public string SquareAvatar { get; set; }
        public string RectangleAvatar { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public BoxSize BoxSize { get; set; }
        public ArticleType ArticleType { get; set; }
        public bool? IsHot { get; set; }
        public string CreatedUserDisplayName { get; set; }
        public string ArticleCategoryColor { get; set; }
        public string ArticleCategoryName { get; set; }
        public string ArticleCategorySeoName { get; set; }
        public int ViewCount { get; set; }
        public DateTime LatestInteraction { get; set; }
        public int CommentCount { get; set; }
        public OpinionLevel? OpinionLevel { get; set; }
    }
}

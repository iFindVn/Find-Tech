using System;
using System.Collections.Generic;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Article : Entity
    {
        public Article()
        {
            LatestInteraction = DateTime.Now;
        }
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
        public bool? IsActived { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public string CreatedUserDisplayName { get; set; }
        public int UpdatedUserId { get; set; }
        public int ArticleCategoryId { get; set; }
        public virtual ArticleCategory ArticleCategory { get; set; }
        public int ViewCount { get; set; }
        public DateTime LatestInteraction { get; set; }
        public int SourceId { get; set; }
        public virtual Source Source { get; set; }
        public virtual ICollection<ContentSection> ContentSections { get; set; }
        public virtual ICollection<Device> RelatedDevices { get; set; }
        public virtual ICollection<Opinion> Opinions { get; set; }
    }
}

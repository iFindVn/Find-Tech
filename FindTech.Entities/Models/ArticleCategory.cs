using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class ArticleCategory : Entity
    {
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
        public string SeoName { get; set; }
        public int Priority { get; set; }
        public string Color { get; set; }
        public bool? IsActived { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsMenu { get; set; }
        public virtual ICollection<Article> Articles { get; set; } 
    }
}

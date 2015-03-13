using System;
using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Areas.BO.Models
{
    public class ArticleCategoryBOViewModel
    {
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
        public int Priority { get; set; }
        public string Color { get; set; }
        public bool? IsActived { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsMenu { get; set; }
    }
}
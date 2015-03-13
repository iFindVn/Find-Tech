using System;
using FindTech.Entities.Models.Enums;

namespace FindTech.Web.Areas.BO.Models
{
    public class BenchmarkGroupBOViewModel
    {
        public int BenchmarkGroupId { get; set; }
        public string BenchmarkGroupName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int? ParentId { get; set; }
        public BenchmarkGroupBOViewModel Parent { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class BenchmarkGroup : Entity
    {
        public int BenchmarkGroupId { get; set; }
        public string BenchmarkGroupName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int? ParentId { get; set; }
        public virtual BenchmarkGroup Parent { get; set; }
        public virtual ICollection<BenchmarkGroup> Children { get; set; }
        public virtual ICollection<Benchmark> Benchmarks { get; set; } 
    }
}

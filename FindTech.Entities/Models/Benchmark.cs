using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class Benchmark : Entity
    {
        public int BenchmarkId { get; set; }
        public string BenchmarkName { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public string WayComparison { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public BenchmarkDataType BenchmarkDataType { get; set; }
        public int BenchmarkGroupId { get; set; }
        public virtual BenchmarkGroup BenchmarkGroup { get; set; }
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
    }
}

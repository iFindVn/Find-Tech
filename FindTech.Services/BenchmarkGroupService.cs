using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface IBenchmarkGroupService : IService<BenchmarkGroup>
    {
    }

    public class BenchmarkGroupService : Service<BenchmarkGroup>, IBenchmarkGroupService
    {
        public BenchmarkGroupService(IRepositoryAsync<BenchmarkGroup> benchmarkGroupRepository)
            : base(benchmarkGroupRepository)
        {
        }
    }
}

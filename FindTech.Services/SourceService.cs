using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface ISourceService : IService<Source>
    {
    }

    public class SourceService : Service<Source>, ISourceService
    {
        public SourceService(IRepositoryAsync<Source> sourceRepository)
            : base(sourceRepository)
        {
        }
    }
}

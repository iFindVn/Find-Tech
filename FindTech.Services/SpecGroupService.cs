using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface ISpecGroupService : IService<SpecGroup>
    {
    }

    public class SpecGroupService : Service<SpecGroup>, ISpecGroupService
    {
        public SpecGroupService(IRepositoryAsync<SpecGroup> specGroupRepository)
            : base(specGroupRepository)
        {
        }
    }
}

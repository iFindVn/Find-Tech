using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface ISpecService : IService<Spec>
    {
    }

    public class SpecService : Service<Spec>, ISpecService
    {
        public SpecService(IRepositoryAsync<Spec> specRepository)
            : base(specRepository)
        {
        }
    }
}

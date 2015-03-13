using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface IBrandService : IService<Brand>
    {
    }

    public class BrandService : Service<Brand>, IBrandService
    {
        public BrandService(IRepositoryAsync<Brand> brandRepository)
            : base(brandRepository)
        {
        }
    }
}

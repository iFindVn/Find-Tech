using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models;
using Repository.Pattern.Repositories;

namespace FindTech.Services
{
    public interface ISpecDetailService:IService<SpecDetail>
    {

    }
    public class SpecDetailService: Service <SpecDetail>, ISpecDetailService
    {
        public SpecDetailService(IRepositoryAsync<SpecDetail> specDetailRepository)
            : base(specDetailRepository)
        {
        }
    }
}

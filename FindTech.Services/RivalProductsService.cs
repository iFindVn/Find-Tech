using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Services
{
    public interface IRivalProductsService : IService<RivalProducts>
    {
    }
    public class RivalProductsService : Service<RivalProducts>, IRivalProductsService
    {
        public RivalProductsService(IRepositoryAsync<RivalProducts> rivalProductsRepository)
            : base(rivalProductsRepository)
        {
        }
    }
}

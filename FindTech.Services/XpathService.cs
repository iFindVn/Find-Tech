using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{


    public interface IXpathService : IService<Xpath>
    {
    }

    public class XpathService : Service<Xpath>, IXpathService
    {
        public XpathService(IRepositoryAsync<Xpath> xpathRepository)
            : base(xpathRepository)
        {
        }
    }
}

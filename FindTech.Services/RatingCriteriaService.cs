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
    public interface IRatingCriteriaService : IService<RatingCriteria>
    {
    }

    public class RatingCriteriaService : Service<RatingCriteria>, IRatingCriteriaService
    {
        public RatingCriteriaService(IRepositoryAsync<RatingCriteria> ratingCriteriaRepository)
            : base(ratingCriteriaRepository)
        {
        }
    }
}

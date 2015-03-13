using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface IOpinionService : IService<Opinion>
    {
        Opinion GetOpinion(int articleId, OpinionLevel opinionLevel);
        IEnumerable<Opinion> GetOpinions(int articleId);
    }

    public class OpinionService : Service<Opinion>, IOpinionService
    {
        private readonly IRepositoryAsync<Opinion> _opinionRepository;
        public OpinionService(IRepositoryAsync<Opinion> opinionRepository)
            : base(opinionRepository)
        {
            _opinionRepository = opinionRepository;
        }

        public Opinion GetOpinion(int articleId, OpinionLevel opinionLevel)
        {
            return _opinionRepository.GetOpinion(articleId, opinionLevel);
        }
        public IEnumerable<Opinion> GetOpinions(int articleId)
        {
            return _opinionRepository.GetOpinions(articleId);
        }
    }
}

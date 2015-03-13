using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Repositories;

namespace FindTech.Repository.Repositories
{
    public static class OpinionRepository
    {
        public static Opinion GetOpinion(this IRepositoryAsync<Opinion> opinionRepository, int articleId, OpinionLevel opinionLevel)
        {
            return opinionRepository.Queryable().FirstOrDefault(a => a.ArticleId == articleId && a.OpinionLevel == opinionLevel);
        }
        public static IEnumerable<Opinion> GetOpinions(this IRepositoryAsync<Opinion> opinionRepository, int articleId)
        {
            return opinionRepository.Queryable().Where(a => a.ArticleId == articleId).AsEnumerable();
        }
    }
}

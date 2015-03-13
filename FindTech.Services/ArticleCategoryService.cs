using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface IArticleCategoryService : IService<ArticleCategory>
    {
    }

    public class ArticleCategoryService : Service<ArticleCategory>, IArticleCategoryService
    {
        public ArticleCategoryService(IRepositoryAsync<ArticleCategory> articleCategoryRepository)
            : base(articleCategoryRepository)
        {
        }
    }
}

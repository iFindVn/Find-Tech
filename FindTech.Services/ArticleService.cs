using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures;
using FindTech.Entities.StoredProcedures.Models;
using FindTech.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface IArticleService : IService<Article>
    {
        IEnumerable<ArticleResult> GetHotArticles(int skip = 0, int take = 10);
        IEnumerable<ArticleResult> GetLatestReviews(int skip = 0, int take = 20);
        IEnumerable<ArticleResult> GetHotReviews(int skip = 0, int take = 20);
        IEnumerable<ArticleResult> GetLatestNewses(int skip = 0, int take = 20);
        Article GetArticle(int articleId);
        Article GetArticleDetail(string seoTitle);
        IEnumerable<ArticleResult> GetListOfArticles(string tags, string categories, ArticleType articleType,
            string whereClauseMore, string orderString = "", int skip = 0, int take = 10);
        IEnumerable<ArticleResult> GetHotNewses(int skip = 0, int take = 10);
    }

    public class ArticleService : Service<Article>, IArticleService
    {
        private readonly IRepositoryAsync<Article> _articleRepository;
        private readonly IFindTechStoredProcedures _findTechStoredProcedures;
        public ArticleService(IRepositoryAsync<Article> articleRepository, IFindTechStoredProcedures findTechStoredProcedures)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _findTechStoredProcedures = findTechStoredProcedures;
        }

        public IEnumerable<ArticleResult> GetHotArticles(int skip = 0, int take = 10)
        {
            return _findTechStoredProcedures.GetListOfArticles("", "", ArticleType.All, " a.IsHot = 1 ", "",
                skip, take);
        }

        public IEnumerable<ArticleResult> GetHotNewses(int skip = 0, int take = 10)
        {
            return _findTechStoredProcedures.GetListOfArticles("", "", ArticleType.News, " a.IsHot = 1 ", "",
                skip, take);
        }

        public IEnumerable<ArticleResult> GetHotReviews(int skip = 0, int take = 20)
        {
            return _findTechStoredProcedures.GetListOfArticles("", "", ArticleType.News, " a.IsHot = 1 ", "",
                skip, take);
        }

        public IEnumerable<ArticleResult> GetLatestReviews(int skip = 0, int take = 20)
        {
            return _findTechStoredProcedures.GetListOfArticles("", "", ArticleType.Reviews, "", "",
                skip, take);
        }

        public IEnumerable<ArticleResult> GetLatestNewses(int skip = 0, int take = 20)
        {
            return _findTechStoredProcedures.GetListOfArticles("", "", ArticleType.News, "", "",
                skip, take);
        }

        public Article GetArticle(int articleId)
        {
            return _articleRepository.GetArticle(articleId);
        }

        public Article GetArticleDetail(string seoTitle)
        {
            return _articleRepository.GetArticleDetail(seoTitle);
        }

        public IEnumerable<ArticleResult> GetListOfArticles(string tags, string categories, ArticleType articleType,
            string whereClauseMore, string orderString = "", int skip = 0, int take = 10)
        {
            return _findTechStoredProcedures.GetListOfArticles(tags, categories, articleType, whereClauseMore,
                orderString, skip, take);
        }
    }
}

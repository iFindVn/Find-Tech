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
        IEnumerable<ArticleResult> GetHotArticles(int skip = 0, int take = 10, string skipArticleIds = "");
        IEnumerable<ArticleResult> GetLatestReviews(int skip = 0, int take = 20, string skipArticleIds = "");
        IEnumerable<ArticleResult> GetHotReviews(int skip = 0, int take = 20, string skipArticleIds = "");
        IEnumerable<ArticleResult> GetLatestNewses(int skip = 0, int take = 20, string skipArticleIds = "");
        Article GetArticle(int articleId);
        Article GetArticleDetail(string seoTitle);
        IEnumerable<ArticleResult> SearchArticles(string keyword, string orderString, int skip, int take);
        IEnumerable<ArticleResult> GetListOfArticles(GetListOfArticlesParameters getListOfArticlesParameters);
        IEnumerable<ArticleResult> GetHotNewses(int skip = 0, int take = 10, string skipArticleIds = "");
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

        public IEnumerable<ArticleResult> GetHotArticles(int skip = 0, int take = 10, string skipArticleIds = "")
        {
            var getListOfArticlesParameters = new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.All,
                Categories = "",
                Tags = "",
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = " a.IsHot = 1 ",
                SkipArticleIds = skipArticleIds
            };
            return _findTechStoredProcedures.GetListOfArticles(getListOfArticlesParameters);
        }

        public IEnumerable<ArticleResult> GetHotNewses(int skip = 0, int take = 10, string skipArticleIds = "")
        {
            var getListOfArticlesParameters = new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.News,
                Categories = "",
                Tags = "",
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = " a.IsHot = 1 ",
                SkipArticleIds = skipArticleIds
            };
            return _findTechStoredProcedures.GetListOfArticles(getListOfArticlesParameters);
        }

        public IEnumerable<ArticleResult> GetHotReviews(int skip = 0, int take = 20, string skipArticleIds = "")
        {
            var getListOfArticlesParameters = new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.Reviews,
                Categories = "",
                Tags = "",
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = " a.IsHot = 1 ",
                SkipArticleIds = skipArticleIds
            };
            return _findTechStoredProcedures.GetListOfArticles(getListOfArticlesParameters);
        }

        public IEnumerable<ArticleResult> GetLatestReviews(int skip = 0, int take = 20, string skipArticleIds = "")
        {
            var getListOfArticlesParameters = new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.Reviews,
                Categories = "",
                Tags = "",
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = "",
                SkipArticleIds = skipArticleIds
            };
            return _findTechStoredProcedures.GetListOfArticles(getListOfArticlesParameters);
        }

        public IEnumerable<ArticleResult> GetLatestNewses(int skip = 0, int take = 20, string skipArticleIds = "")
        {
            var getListOfArticlesParameters = new GetListOfArticlesParameters
            {
                ArticleType = ArticleType.News,
                Categories = "",
                Tags = "",
                OrderString = "",
                Skip = skip,
                Take = take,
                WhereClauseMore = "",
                SkipArticleIds = skipArticleIds
            };
            return _findTechStoredProcedures.GetListOfArticles(getListOfArticlesParameters);
        }

        public Article GetArticle(int articleId)
        {
            return _articleRepository.GetArticle(articleId);
        }

        public Article GetArticleDetail(string seoTitle)
        {
            return _articleRepository.GetArticleDetail(seoTitle);
        }

        public IEnumerable<ArticleResult> GetListOfArticles(GetListOfArticlesParameters getListOfArticlesParameters)
        {
            return _findTechStoredProcedures.GetListOfArticles(getListOfArticlesParameters);
        }

        public IEnumerable<ArticleResult> SearchArticles(string keyword, string orderString, int skip, int take)
        {
            return _findTechStoredProcedures.SearchArticles(keyword, orderString, skip, take);
        }
    }
}

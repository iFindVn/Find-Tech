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
    public interface IArticleService : IService<Article>
    {
        IEnumerable<Article> GetHotArticles();
        IEnumerable<Article> GetLatestReviews(int skip = 0, int take = 20);
        IEnumerable<Article> GetPopularReviews(int skip = 0, int take = 20);
        Article GetArticle(int articleId);
        Article GetArticleDetail(string seoTitle);
    }

    public class ArticleService : Service<Article>, IArticleService
    {
        private readonly IRepositoryAsync<Article> _articleRepository;
        public ArticleService(IRepositoryAsync<Article> articleRepository)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IEnumerable<Article> GetHotArticles()
        {
            return
                _articleRepository.GetHotArticles();
        }

        public IEnumerable<Article> GetLatestReviews(int skip = 0, int take = 20)
        {
            return
                _articleRepository.GetLatestReviews(skip, take);
        }

        public IEnumerable<Article> GetPopularReviews(int skip = 0, int take = 20)
        {
            return
                _articleRepository.GetPopularReviews(skip, take);
        }

        public Article GetArticle(int articleId)
        {
            return _articleRepository.GetArticle(articleId);
        }

        public Article GetArticleDetail(string seoTitle)
        {
            return _articleRepository.GetArticleDetail(seoTitle);
        }
    }
}

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Repositories;

namespace FindTech.Repository.Repositories
{
    public static class ArticleRepository
    {
        public static IEnumerable<Article> GetHotArticles(this IRepositoryAsync<Article> articleRepository)
        {
            return
                articleRepository.Queryable()
                    .Where(a => a.IsHot == true)
                    .OrderByDescending(a => a.Priority)
                    .ThenByDescending(a => a.PublishedDate)
                    .Include(a => a.Source)
                    .Include(a => a.ArticleCategory)
                    .AsEnumerable();
        }

        public static IEnumerable<Article> GetLatestReviews(this IRepositoryAsync<Article> articleRepository, int skip = 0, int take = 20)
        {
            return
                articleRepository.Queryable()
                    .Where(a => a.ArticleType == ArticleType.Reviews && a.IsHot != true)
                    .OrderByDescending(a => a.Priority)
                    .ThenByDescending(a => a.PublishedDate)
                    .Skip(skip)
                    .Take(take)
                    .Include(a => a.Source)
                    .Include(a => a.ArticleCategory)
                    .AsEnumerable();
        }

        public static IEnumerable<Article> GetPopularReviews(this IRepositoryAsync<Article> articleRepository, int skip = 0, int take = 20)
        {
            return
                articleRepository.Queryable()
                    .Where(a => a.ArticleType == ArticleType.Reviews && a.IsHot != true)
                    .OrderByDescending(a => a.ViewCount)
                    .ThenByDescending(a => a.Priority)
                    .ThenByDescending(a => a.PublishedDate)
                    .Skip(skip)
                    .Take(take)
                    .Include(a => a.Source)
                    .Include(a => a.ArticleCategory)
                    .AsEnumerable();
        }

        public static Article GetArticle(this IRepositoryAsync<Article> articleRepository, int articleId)
        {
            return articleRepository.Queryable().Include(a => a.Source).Include(a => a.ArticleCategory).Include(a => a.Opinions).FirstOrDefault(a => a.ArticleId == articleId);
        }

        public static Article GetArticleDetail(this IRepositoryAsync<Article> articleRepository, string seoTitle)
        {
            return articleRepository.Queryable().Include(a => a.Source).Include(a => a.ArticleCategory).Include(a => a.Opinions).FirstOrDefault(a => a.SeoTitle == seoTitle);
        }
    }
}

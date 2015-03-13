using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Repositories;

namespace FindTech.Repository.Repositories
{
    public static class ContentSectionRepository
    {
        public static IEnumerable<ContentSection> GetContentSectionPages(this IRepositoryAsync<ContentSection> contentSectionRepository, int articleId, int currentPage)
        {
            return contentSectionRepository.Queryable().Include(a => a.Article)
                .Where(a => a.ArticleId == articleId).OrderBy(a => a.PageNumber)
                .AsEnumerable();
        }

        public static IEnumerable<ContentSection> GetContentSections(this IRepositoryAsync<ContentSection> contentSectionRepository, int articleId, int page)
        {
            return contentSectionRepository.Queryable().Include(a => a.Images)
                .Where(a => a.ArticleId == articleId && a.PageNumber == page)
                .AsEnumerable();
        }
    }
}

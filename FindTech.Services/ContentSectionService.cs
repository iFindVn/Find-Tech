using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FindTech.Entities.Models;
using FindTech.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{
    public interface IContentSectionService : IService<ContentSection>
    {
        IEnumerable<ContentSection> GetContentSections(int articleId, int page);
        IEnumerable<object> GetContentSectionPages(int articleId, int currentPage);
    }

    public class ContentSectionService : Service<ContentSection>, IContentSectionService
    {
        private readonly IRepositoryAsync<ContentSection> _contentSectionRepository;
        public ContentSectionService(IRepositoryAsync<ContentSection> contentSectionRepository)
            : base(contentSectionRepository)
        {
            _contentSectionRepository = contentSectionRepository;
        }
        public IEnumerable<object> GetContentSectionPages(int articleId, int currentPage)
        {
            var contentSections = _contentSectionRepository.GetContentSectionPages(articleId, currentPage);
            return
                contentSections.GroupBy(a => a.PageNumber, a => a.SectionTitle,
                    (key, p) =>
                        new {PageNumber = key, PageName = string.Join(" & ", p), IsCurrentPage = key == currentPage});
        }

        public IEnumerable<ContentSection> GetContentSections(int articleId, int page)
        {
            return _contentSectionRepository.GetContentSections(articleId, page);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures.Models;

namespace FindTech.Entities.StoredProcedures
{
    public interface IFindTechStoredProcedures
    {
        #region Article Stored Procedures
        IEnumerable<SearchArticlesResult> SearchArticles(string keyword, string orderString = "");

        IEnumerable<ArticleResult> GetListOfArticles(string tags, string categories, ArticleType articleType,
            string whereClauseMore, string orderString = "", int skip = 0, int take = 10);

        #endregion
    }
}

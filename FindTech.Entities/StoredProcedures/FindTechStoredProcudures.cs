using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures;
using FindTech.Entities.StoredProcedures.Models;

namespace FindTech.Entities
{
    public partial class FindTechContext : IFindTechStoredProcedures
    {
        #region Article Stored Procedures

        public IEnumerable<SearchArticlesResult> SearchArticles(string keyword, string orderString = "")
        {
            var keywordParameter = keyword != null ?
            new SqlParameter("@keyword", keyword) :
            new SqlParameter("@keyword", typeof(string));

            var orderStringParameter = orderString != null ?
            new SqlParameter("@orderString", orderString) :
            new SqlParameter("@orderString", typeof(string));

            return Database.SqlQuery<SearchArticlesResult>("SP_Article_SearchArticles @keyword, @orderString", keywordParameter, orderStringParameter);
        }

        public IEnumerable<ArticleResult> GetListOfArticles(string tags, string categories, ArticleType articleType, string whereClauseMore, string orderString = "", int skip = 0, int take = 10)
        {
            var tagsParameter = tags != null ?
            new SqlParameter("@tags", tags) :
            new SqlParameter("@tags", typeof(string));

            var categoriesParameter = categories != null ?
            new SqlParameter("@categories", categories) :
            new SqlParameter("@categories", typeof(string));

            var articleTypeParameter = new SqlParameter("@articleType", (int)articleType);

            var whereClauseMoreParameter = whereClauseMore != null ?
            new SqlParameter("@whereClauseMore", whereClauseMore) :
            new SqlParameter("@whereClauseMore", typeof(string));

            var orderStringParameter = orderString != null ?
            new SqlParameter("@orderString", orderString) :
            new SqlParameter("@orderString", typeof(string));

            var skipParameter = new SqlParameter("@skip", skip);

            var takeParameter = new SqlParameter("@take", take);

            return
                Database.SqlQuery<ArticleResult>(
                    "SP_Article_GetListOfArticles @tags, @categories, @articleType, @whereClauseMore, @orderString, @skip, @take",
                    tagsParameter, categoriesParameter, articleTypeParameter, whereClauseMoreParameter,
                    orderStringParameter, skipParameter, takeParameter).AsEnumerable();
        }
        #endregion
    }
}

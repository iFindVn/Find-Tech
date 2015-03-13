using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            return Database.SqlQuery<SearchArticlesResult>("search_order_2 @keyword, @orderString", keywordParameter, orderStringParameter);
        }
        #endregion
    }
}

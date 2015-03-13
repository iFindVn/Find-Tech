using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.StoredProcedures;
using FindTech.Entities.StoredProcedures.Models;

namespace FindTech.Services
{
    public interface IStoredProcedureService
    {
        #region Article Stored Procedures
        IEnumerable<SearchArticlesResult> SearchArticles(string keyword, string orderString = "");
        #endregion
    }
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly IFindTechStoredProcedures _storedProcedureService;
        public StoredProcedureService(IFindTechStoredProcedures storedProcedureService)
        {
            this._storedProcedureService = storedProcedureService;
        }
        #region Article Stored Procedures

        public IEnumerable<SearchArticlesResult> SearchArticles(string keyword, string orderString)
        {
            return _storedProcedureService.SearchArticles(keyword, orderString);
        }
        #endregion
    }
}

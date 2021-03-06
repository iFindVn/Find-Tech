﻿using System;
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
        IEnumerable<ArticleResult> SearchArticles(string keyword, string orderString, int skip, int take);

        IEnumerable<ArticleResult> GetListOfArticles(GetListOfArticlesParameters getListOfArticlesParameters);

        IEnumerable<CommentResult> GetListOfComments(int objectId, ObjectType objectType, int skip, int take, ref int commentCount);

        #endregion
    }
}

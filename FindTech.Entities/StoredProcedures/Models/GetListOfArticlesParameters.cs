using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;

namespace FindTech.Entities.StoredProcedures.Models
{
    public class GetListOfArticlesParameters
    {
        public string Tags { get; set; } 
        public string Categories { get; set; } 
        public ArticleType ArticleType { get; set; }
        public string WhereClauseMore { get; set; }
        public string OrderString { get; set; }
        public string SkipArticleIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}

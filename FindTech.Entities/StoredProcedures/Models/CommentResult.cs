using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;

namespace FindTech.Entities.StoredProcedures.Models
{
    public class CommentResult
    {
        public int CommentId { get; set; }
        public string CommentatorEmail { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int ObjectId { get; set; }
        public ObjectType ObjectType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ReplyCount { get; set; }
        public IEnumerable<CommentResult> Replies { get; set; } 
    }
}

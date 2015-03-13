using FindTech.Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTech.Web.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public string CommentatorEmail { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int ObjectId { get; set; }
        public ObjectType ObjectType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<CommentModel> Replies { get; set; } 
    }
}
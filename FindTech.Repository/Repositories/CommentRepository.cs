using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Repositories;

namespace FindTech.Repository.Repositories
{
    public static class CommentRepository
    {
        public static IEnumerable<Comment> GetComments(this IRepositoryAsync<Comment> commentRepository, int objectId, ObjectType objectType, int skip = 0, int take = 5)
        {
            return commentRepository.Queryable()
                .Where(a => a.ObjectId == objectId && a.ObjectType == objectType)
                .OrderByDescending(a => a.CreatedDate)
                .Skip(skip).Take(take)
                .AsEnumerable();
        }

        public static int GetCommentCount(this IRepositoryAsync<Comment> commentRepository, int objectId, ObjectType objectType)
        {
            return commentRepository.Queryable()
                .Count(a => a.ObjectId == objectId && a.ObjectType == objectType);
        }

        public static IEnumerable<Comment> GetReplies(this IRepositoryAsync<Comment> commentRepository, int commentId, int skip = 0, int take = 2)
        {
            return commentRepository.Queryable()
                .Where(r => r.ObjectId == commentId && r.ObjectType == ObjectType.Comment)
                .OrderByDescending(r => r.CreatedDate)
                .Skip(skip).Take(take)
                .AsEnumerable();
        }
    }
}

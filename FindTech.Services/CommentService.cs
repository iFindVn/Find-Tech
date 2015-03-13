using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Services
{
    public interface ICommentService : IService<Comment>
    {
        IEnumerable<Comment> GetComments(int objectId, ObjectType objectType);
        IEnumerable<Comment> GetReplies(int commentId);
    }
    public class CommentService : Service<Comment>, ICommentService
    {
        private readonly IRepositoryAsync<Comment> _commentRepository;
        public CommentService(IRepositoryAsync<Comment> commentRepository)
            : base(commentRepository)
        {
            this._commentRepository = commentRepository;
        }

        public IEnumerable<Comment> GetComments(int objectId, ObjectType objectType)
        {
            return _commentRepository.GetComments(objectId, objectType);
        }

        public IEnumerable<Comment> GetReplies(int commentId)
        {
            return _commentRepository.GetReplies(commentId);
        }
    }
}

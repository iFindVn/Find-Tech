using FindTech.Entities.Models;
using FindTech.Entities.Models.Enums;
using FindTech.Entities.StoredProcedures;
using FindTech.Entities.StoredProcedures.Models;
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
        int GetCommentCount(int objectId, ObjectType objectType);
        IEnumerable<Comment> GetReplies(int commentId);
        IEnumerable<CommentResult> GetListOfComments(int objectId, ObjectType objectType, int skip, int take, ref int commentCount);
    }
    public class CommentService : Service<Comment>, ICommentService
    {
        private readonly IRepositoryAsync<Comment> _commentRepository;
        private readonly IFindTechStoredProcedures _findTechStoredProcedures;
        public CommentService(IRepositoryAsync<Comment> commentRepository, IFindTechStoredProcedures findTechStoredProcedures)
            : base(commentRepository)
        {
            this._commentRepository = commentRepository;
            _findTechStoredProcedures = findTechStoredProcedures;
        }

        public IEnumerable<Comment> GetComments(int objectId, ObjectType objectType)
        {
            return _commentRepository.GetComments(objectId, objectType);
        }

        public int GetCommentCount(int objectId, ObjectType objectType)
        {
            return _commentRepository.GetCommentCount(objectId, objectType);
        }

        public IEnumerable<Comment> GetReplies(int commentId)
        {
            return _commentRepository.GetReplies(commentId);
        }

        public IEnumerable<CommentResult> GetListOfComments(int objectId, ObjectType objectType, int skip, int take, ref int commentCount)
        {
            return _findTechStoredProcedures.GetListOfComments(objectId, objectType, skip, take, ref commentCount);
        } 
    }
}

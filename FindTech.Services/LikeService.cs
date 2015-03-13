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

    public interface ILikeService : IService<Like>
    {
        int GetLikeCount(int objectId, ObjectType objectType);
    }
    public class LikeService: Service<Like>, ILikeService
    {
        private readonly IRepositoryAsync<Like> _likeRepository;
        public LikeService(IRepositoryAsync<Like> likeRepository)
            : base(likeRepository)
        {
            this._likeRepository = likeRepository;
        }

        public int GetLikeCount(int objectId, ObjectType objectType)
        {
            return _likeRepository.GetLikeCount(objectId, objectType);
        }
    }
}

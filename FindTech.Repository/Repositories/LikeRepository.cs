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
    public static class LikeRepository
    {
        public static int GetLikeCount(this IRepositoryAsync<Like> likeRepository, int objectId, ObjectType objectType)
        {
            return likeRepository.Queryable().Count(l => l.ObjectId == objectId && l.ObjectType == objectType);
        }
    }
}

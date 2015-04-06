using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Services
{
    public interface IVideoService : IService<Video>
    {
    }

    public class VideoService : Service<Video>, IVideoService
    {
        public VideoService(IRepositoryAsync<Video> videoRepository)
            : base(videoRepository)
        {
        }
    }
}

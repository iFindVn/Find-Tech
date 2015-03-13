using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTech.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace FindTech.Services
{


    public interface IImageService : IService<Image>
    {
    }

    public class ImageService : Service<Image>, IImageService
    {
        public ImageService(IRepositoryAsync<Image> imageRepository)
            : base(imageRepository)
        {
        }
    }
}

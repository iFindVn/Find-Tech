using System.Collections.Generic;
using FindTech.Entities.Models;

namespace FindTech.Web.Models
{
    public class ImageListViewModel
    {
        public int ContentSectionId { get; set; }
        public IEnumerable<Image> Images { get; set; } 
    }
}
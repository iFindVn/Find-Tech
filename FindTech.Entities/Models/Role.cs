using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Entities.Models
{
    class Role:Entity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}

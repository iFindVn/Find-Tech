using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FindTech.Entities.Models.Enums;
using Repository.Pattern.Ef6;

namespace FindTech.Entities.Models
{
    public class RivalProducts : Entity
    {
        [Key]
        [Column(Order = 1)]
        public int DeviceHome { get; set; }

        [Key]
        [Column(Order = 2)]
        public int DeviceRival { get; set; }
        public int Priority { get; set; }

        public bool? IsActived { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

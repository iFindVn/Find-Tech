using System;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;

namespace Repository.Pattern.Ef6
{
    public abstract class Entity : IObjectState
    {
        protected Entity()
        {
            CreatedDate = DateTime.Now;
        }
        [NotMapped]
        public ObjectState ObjectState { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
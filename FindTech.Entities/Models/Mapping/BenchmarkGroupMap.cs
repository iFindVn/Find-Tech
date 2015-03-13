using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTech.Entities.Models.Mapping
{
    public class BenchmarkGroupMap : EntityTypeConfiguration<BenchmarkGroup>
    {
        public BenchmarkGroupMap()
        {
            HasOptional(a => a.Parent).WithMany(a => a.Children).HasForeignKey(a => a.ParentId);
        }
    }
}

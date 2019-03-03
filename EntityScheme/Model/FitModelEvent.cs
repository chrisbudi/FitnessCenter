using DataObjects.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheme.Model
{
    public class FitModelEvent : FitModelMaster
    {
        public virtual DbSet<trEventScore> trEventScores { get; set; }

        public virtual DbSet<strPersonEvent> strPersonEvents { get; set; }

        public virtual DbSet<trEventStep> trEventSteps { get; set; }

        public virtual DbSet<trPersonEvent> trPersonEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}

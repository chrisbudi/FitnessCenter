using System.Data.Entity;
using DataObjects.Entity;

namespace Scheme.Model
{
    public partial class FitModelAktivitas : FitModelMaster
    {

        public virtual DbSet<trAktifitasKela> trAktifitasKelas { get; set; }
        public virtual DbSet<trAktifitasMember> trAktifitasMembers { get; set; }
        public virtual DbSet<trPlanKela> trPlanKelas { get; set; }
        public virtual DbSet<trCinCout> trCinCouts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}

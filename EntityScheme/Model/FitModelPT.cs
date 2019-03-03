using System.Data.Entity;
using DataObjects.Entity;

namespace Scheme.Model
{
    public partial class FitModelPT : FitModelMaster
    {

        public virtual DbSet<strKlaimPT> strKlaimPTs { get; set; }
        public virtual DbSet<trAktifitasMember> trAktifitasMembers { get; set; }
        public virtual DbSet<trPersonalTrainer> trPersonalTrainers { get; set; }
        public virtual DbSet<trTTD> trTTDs { get; set; }
        public virtual DbSet<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}

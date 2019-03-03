using System.Data.Entity;
using DataObjects.Entity;

namespace Scheme.Model
{
    public class FitModelKeuPostTrans : FitModelMaster
    {
        public virtual DbSet<trPersonalTrainer> trPersonalTrainers { get; set; }
        public virtual DbSet<trMembership> trMemberships { get; set; }
        public virtual DbSet<trPembayaran> trPembayarans { get; set; }
        public virtual DbSet<strPaymentMember> strPaymentMembers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
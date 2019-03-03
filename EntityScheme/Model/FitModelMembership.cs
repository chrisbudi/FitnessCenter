using System.Data.Entity;
using DataObjects.Entity;

namespace Scheme.Model
{
    public class FitModelMembership : FitModelMaster
    {
        // Model Membership
        public virtual DbSet<strAktivitasSale> strAktivitasSales { get; set; }
        public virtual DbSet<strPayment> strPayments { get; set; }
        public virtual DbSet<strPaymentMember> strPaymentMembers { get; set; }
        public virtual DbSet<trMembership> trMemberships { get; set; }
        public virtual DbSet<trPaymentWith> trPaymentWiths { get; set; }
        public virtual DbSet<trTTD> trTTDs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<strAktivitasSale>()
                .Property(e => e.aktivitasstap)
                .IsFixedLength();

            modelBuilder.Entity<tMemberState>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.tMemberState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trMembership>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.trMembership)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tSalesAction>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.tSalesAction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<strPaymentMember>()
                .Property(e => e.TimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<strPaymentMember>()
                .HasMany(e => e.strPayments)
                .WithRequired(e => e.strPaymentMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPaymentType>()
                .HasMany(e => e.trPaymentWiths)
                .WithRequired(e => e.tPaymentType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPaymentWith>()
                .Property(e => e.payAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPaymentWith>()
                .HasMany(e => e.strPayments)
                .WithRequired(e => e.trPaymentWith)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.trTTDs)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);


        }
    }
}
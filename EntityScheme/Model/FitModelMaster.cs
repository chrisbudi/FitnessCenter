using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DataObjects.Entity;

namespace Scheme.Model
{
    public class FitModelMaster : DbContext
    {
        public FitModelMaster()
            : base("name=FitModelEntity")
        {
            Database.SetInitializer<FitModelMaster>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 3000;
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<strLocBO> strLocBOes { get; set; }
        public virtual DbSet<strLocMember> strLocMembers { get; set; }
        public virtual DbSet<tActionPT> tActionPTs { get; set; }
        public virtual DbSet<tBank> tBanks { get; set; }
        public virtual DbSet<tKela> tKelas { get; set; }
        public virtual DbSet<tLocFitnessCenter> tLocFitnessCenters { get; set; }
        public virtual DbSet<tMember> tMembers { get; set; }
        public virtual DbSet<tMemberState> tMemberStates { get; set; }
        public virtual DbSet<tMemberType> tMemberTypes { get; set; }
        public virtual DbSet<tPaketPT> tPaketPTs { get; set; }
        public virtual DbSet<tPaymentType> tPaymentTypes { get; set; }
        public virtual DbSet<tPerson> tPersons { get; set; }
        public virtual DbSet<tPosisi> tPosisis { get; set; }
        public virtual DbSet<tRuangKela> tRuangKelas { get; set; }
        public virtual DbSet<tSalesAction> tSalesActions { get; set; }
        public virtual DbSet<tStatusMember> tStatusMembers { get; set; }
        public virtual DbSet<tTypeStatusCinCout> tTypeStatusCinCouts { get; set; }
        public virtual DbSet<tUserBackOffice> tUserBackOffices { get; set; }
        public virtual DbSet<Excel> Excels { get; set; }
        public virtual DbSet<ExcelMembership> ExcelMemberships { get; set; }
        public virtual DbSet<ExcelFlashProject> ExcelFlashProjects { get; set; }
        public virtual DbSet<tRoleEvent> tRoleEvents { get; set; }

        public virtual DbSet<tEvent> tEvents { get; set; }
        public virtual DbSet<tEventScore> tEventScores { get; set; }
        public virtual DbSet<tEventStep> tEventSteps { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany(e => e.AspFormAuthorizations)
                .WithRequired(e => e.ApplicationGroup)
                .HasForeignKey(e => e.GroupId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<AspForm>()
                .HasMany(e => e.AspForm1)
                .WithOptional(e => e.AspForm2)
                .HasForeignKey(e => e.parent_ID);

            modelBuilder.Entity<AspForm>()
                .HasMany(e => e.AspFormAuthorizations)
                .WithRequired(e => e.AspForm)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetRole)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<strAktivitasSale>()
                .Property(e => e.aktivitasstap)
                .IsFixedLength();

            modelBuilder.Entity<strLocBO>()
                .HasMany(e => e.trCinCouts)
                .WithRequired(e => e.strLocBO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<strPaymentMember>()
                .Property(e => e.TimeStamp)
                .IsFixedLength();

            modelBuilder.Entity<strPaymentMember>()
                .HasMany(e => e.strPayments)
                .WithRequired(e => e.strPaymentMember)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tActionPT>()
                .HasMany(e => e.trPlanAktifitasPTs)
                .WithRequired(e => e.tActionPT)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tEvent>()
                .HasMany(e => e.tEventSteps)
                .WithRequired(e => e.tEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tEvent>()
                .HasMany(e => e.trPersonEvents)
                .WithRequired(e => e.tEvent)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tKela>()
                .HasMany(e => e.trPlanKelas)
                .WithRequired(e => e.tKela)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.strLocBOes)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.strLocMembers)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.tMemberTypes)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.trAktifitasMembers)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.trMemberships)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.trPersonalTrainers)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.trPlanAktifitasPTs)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tLocFitnessCenter>()
                .HasMany(e => e.trPlanKelas)
                .WithRequired(e => e.tLocFitnessCenter)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tMember>()
                .HasMany(e => e.strLocMembers)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trAktifitasMembers)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trMemberships)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trPlanAktifitasPTs)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trTTDs)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMemberState>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.tMemberState)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tMemberType>()
                .Property(e => e.Biaya)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tMemberType>()
                .Property(e => e.Periode)
                .IsFixedLength();

            modelBuilder.Entity<tMemberType>()
                .Property(e => e.prFix)
                .IsFixedLength();

            modelBuilder.Entity<tPaketPT>()
                .Property(e => e.tPaketPTID)
                .IsFixedLength();

            modelBuilder.Entity<tPaketPT>()
                .Property(e => e.PPTHarga)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tPaketPT>()
                .HasMany(e => e.trPersonalTrainers)
                .WithRequired(e => e.tPaketPT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPaymentType>()
                .HasMany(e => e.trPaymentWiths)
                .WithRequired(e => e.tPaymentType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .Property(e => e.PGender)
                .IsFixedLength();

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.tMembers)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.trMemberships)
                .WithRequired(e => e.tPerson)
                .HasForeignKey(e => e.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.trPersonEvents)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.trTTDs)
                .WithRequired(e => e.tPerson)
                .HasForeignKey(e => e.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasOptional(e => e.tUserBackOffice)
                .WithRequired(e => e.tPerson);

            modelBuilder.Entity<tPosisi>()
                .HasMany(e => e.tUserBackOffices)
                .WithRequired(e => e.tPosisi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trAktifitasMember>()
                .Property(e => e.Status);

            modelBuilder.Entity<trAktifitasMember>()
                .HasMany(e => e.strKlaimPTs)
                .WithRequired(e => e.trAktifitasMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trCinCout>()
                .Property(e => e.RefCinCout)
                .IsFixedLength();

            modelBuilder.Entity<trMembership>()
                .Property(e => e.MSTime)
                .IsFixedLength();

            modelBuilder.Entity<trMembership>()
                .Property(e => e.Subtotal)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trMembership>()
                .Property(e => e.Disc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trMembership>()
                .Property(e => e.Total)
                .HasPrecision(18, 0);


            modelBuilder.Entity<trMembership>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.trMembership)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trMembership>()
                .HasMany(e => e.trMembership1)
                .WithOptional(e => e.trMembership2)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<trMembership>()
                .HasMany(e => e.trPembayarans)
                .WithRequired(e => e.trMembership)
                .HasForeignKey(e => e.ID_Transaksi)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<tRoleEvent>()
                .HasMany(e => e.trPersonEvents)
                .WithRequired(e => e.tRoleEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPaymentWith>()
                .Property(e => e.payAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPaymentWith>()
                .HasMany(e => e.strPayments)
                .WithRequired(e => e.trPaymentWith)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPembayaran>()
                .Property(e => e.TS)
                .IsFixedLength();

            modelBuilder.Entity<trPembayaran>()
                .Property(e => e.D_JUMLAH)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPembayaran>()
                .Property(e => e.CC)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPembayaran>()
                .Property(e => e.Debit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPembayaran>()
                .Property(e => e.Cash)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPersonalTrainer>()
                .Property(e => e.tPaketPTID)
                .IsFixedLength();

            modelBuilder.Entity<trPersonalTrainer>()
                .Property(e => e.PTSubtotal)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPersonalTrainer>()
                .Property(e => e.PTDiskon)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPersonalTrainer>()
                .Property(e => e.PTTotal)
                .HasPrecision(18, 0);


            modelBuilder.Entity<trPersonalTrainer>()
                .HasMany(e => e.strKlaimPTs)
                .WithRequired(e => e.trPersonalTrainer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPersonalTrainer>()
                .HasMany(e => e.trPembayarans)
                .WithRequired(e => e.trPersonalTrainer)
                .HasForeignKey(e => e.ID_Transaksi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPersonEvent>()
                .HasOptional(e => e.strPersonEvent)
                .WithRequired(e => e.trPersonEvent);

            modelBuilder.Entity<trPersonEvent>()
                .HasMany(e => e.trEventSteps)
                .WithRequired(e => e.trPersonEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPlanAktifitasPT>()
                .HasMany(e => e.trPlanAktifitasPT1)
                .WithOptional(e => e.trPlanAktifitasPT2)
                .HasForeignKey(e => e.PalnAktifitasPTParentID);

            modelBuilder.Entity<trPlanKela>()
                .HasMany(e => e.trAktifitasKelas)
                .WithRequired(e => e.trPlanKela)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tSalesAction>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.tSalesAction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tStatusMember>()
                .HasMany(e => e.trMemberships)
                .WithRequired(e => e.tStatusMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tTypeStatusCinCout>()
                .HasMany(e => e.trCinCouts)
                .WithRequired(e => e.tTypeStatusCinCout)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.strLocBOes)
                .WithRequired(e => e.tUserBackOffice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trAktifitasMembers)
                .WithRequired(e => e.tUserBackOffice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trCinCouts)
                .WithRequired(e => e.tUserBackOffice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trMemberships)
                .WithRequired(e => e.tUserBackOffice)
                .HasForeignKey(e => e.PersonBOIDADM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trMemberships1)
                .WithRequired(e => e.tUserBackOffice1)
                .HasForeignKey(e => e.PersonBOIDSales)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPembayarans)
                .WithRequired(e => e.tUserBackOffice)
                .HasForeignKey(e => e.PersonBOIDFin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPersonalTrainers)
                .WithRequired(e => e.tUserBackOffice)
                .HasForeignKey(e => e.PersonBOIDPT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPersonalTrainers1)
                .WithRequired(e => e.tUserBackOffice1)
                .HasForeignKey(e => e.PersonBOIDAdm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPersonalTrainers2)
                .WithRequired(e => e.tUserBackOffice2)
                .HasForeignKey(e => e.PersonBOIDSales)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPlanAktifitasPTs)
                .WithRequired(e => e.tUserBackOffice)
                .HasForeignKey(e => e.PersonBOIDPT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPlanAktifitasPTs1)
                .WithRequired(e => e.tUserBackOffice1)
                .HasForeignKey(e => e.PersonBOID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPlanKelas)
                .WithRequired(e => e.tUserBackOffice)
                .HasForeignKey(e => e.PersonBOIDInstruktur)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPlanKelas1)
                .WithRequired(e => e.tUserBackOffice1)
                .HasForeignKey(e => e.PersonBOIDAdm)
                .WillCascadeOnDelete(false);

        }
    }
}
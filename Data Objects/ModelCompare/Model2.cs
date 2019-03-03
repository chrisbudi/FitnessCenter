namespace DataObjects.ModelCompare
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Entity")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<ApplicationGroupRole> ApplicationGroupRoles { get; set; }
        public virtual DbSet<ApplicationGroup> ApplicationGroups { get; set; }
        public virtual DbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }
        public virtual DbSet<AspForm> AspForms { get; set; }
        public virtual DbSet<AspFormAuthorization> AspFormAuthorizations { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<strAktivitasSale> strAktivitasSales { get; set; }
        public virtual DbSet<strKlaimPT> strKlaimPTs { get; set; }
        public virtual DbSet<strLocBO> strLocBOes { get; set; }
        public virtual DbSet<strLocMember> strLocMembers { get; set; }
        public virtual DbSet<strPayment> strPayments { get; set; }
        public virtual DbSet<strPaymentMember> strPaymentMembers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
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
        public virtual DbSet<trAktifitasKela> trAktifitasKelas { get; set; }
        public virtual DbSet<trAktifitasMember> trAktifitasMembers { get; set; }
        public virtual DbSet<trCinCout> trCinCouts { get; set; }
        public virtual DbSet<trMembership> trMemberships { get; set; }
        public virtual DbSet<trPaymentWith> trPaymentWiths { get; set; }
        public virtual DbSet<trPersonalTrainer> trPersonalTrainers { get; set; }
        public virtual DbSet<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }
        public virtual DbSet<trPlanKela> trPlanKelas { get; set; }
        public virtual DbSet<trTTD> trTTDs { get; set; }
        public virtual DbSet<tSalesAction> tSalesActions { get; set; }
        public virtual DbSet<tStatusMember> tStatusMembers { get; set; }
        public virtual DbSet<tTypeStatusCinCout> tTypeStatusCinCouts { get; set; }
        public virtual DbSet<tUserBackOffice> tUserBackOffices { get; set; }
        public virtual DbSet<Excel> Excels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationGroup>()
                .HasMany(e => e.AspFormAuthorizations)
                .WithRequired(e => e.ApplicationGroup)
                .HasForeignKey(e => e.GroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.Module)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.desciption)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.controller)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.action)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.area)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.parameter)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.statusCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.iconclass)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .HasMany(e => e.AspForm1)
                .WithOptional(e => e.AspForm2)
                .HasForeignKey(e => e.parent_ID);

            modelBuilder.Entity<AspForm>()
                .HasMany(e => e.AspFormAuthorizations)
                .WithRequired(e => e.AspForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetRole)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<AspNetUserRole>()
                .Property(e => e.Description)
                .IsUnicode(false);

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
                .Property(e => e.BOID)
                .IsUnicode(false);

            modelBuilder.Entity<strLocBO>()
                .HasMany(e => e.trCinCouts)
                .WithRequired(e => e.strLocBO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<strLocMember>()
                .Property(e => e.MemberID)
                .IsUnicode(false);

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

            modelBuilder.Entity<tKela>()
                .Property(e => e.KelasID)
                .IsUnicode(false);

            modelBuilder.Entity<tKela>()
                .Property(e => e.KNamaKelas)
                .IsUnicode(false);

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
                .Property(e => e.MemberID)
                .IsUnicode(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.strLocMembers)
                .WithRequired(e => e.tMember)
                .HasForeignKey(e => e.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.strLocMembers1)
                .WithRequired(e => e.tMember1)
                .HasForeignKey(e => e.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trAktifitasMembers)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trPersonalTrainers)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMember>()
                .HasMany(e => e.trPlanAktifitasPTs)
                .WithRequired(e => e.tMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMemberState>()
                .HasMany(e => e.strAktivitasSales)
                .WithRequired(e => e.tMemberState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tMemberType>()
                .Property(e => e.MemberType)
                .IsUnicode(false);

            modelBuilder.Entity<tMemberType>()
                .Property(e => e.Biaya)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tMemberType>()
                .Property(e => e.Periode)
                .IsFixedLength();

            modelBuilder.Entity<tMemberType>()
                .Property(e => e.prFix)
                .IsFixedLength()
                .IsUnicode(false);

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
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.tMembers)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.trMemberships)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.trTTDs)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPerson>()
                .HasMany(e => e.tUserBackOffices)
                .WithRequired(e => e.tPerson)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tPosisi>()
                .HasMany(e => e.tUserBackOffices)
                .WithRequired(e => e.tPosisi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trAktifitasMember>()
                .Property(e => e.MemberID)
                .IsUnicode(false);

            modelBuilder.Entity<trAktifitasMember>()
                .Property(e => e.BOID)
                .IsUnicode(false);

            modelBuilder.Entity<trAktifitasMember>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<trAktifitasMember>()
                .HasMany(e => e.strKlaimPTs)
                .WithRequired(e => e.trAktifitasMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trCinCout>()
                .Property(e => e.RefCinCout)
                .IsFixedLength();

            modelBuilder.Entity<trCinCout>()
                .Property(e => e.BOID)
                .IsUnicode(false);

            modelBuilder.Entity<trMembership>()
                .Property(e => e.MSTime)
                .IsFixedLength();

            modelBuilder.Entity<trMembership>()
                .Property(e => e.MemberID)
                .IsUnicode(false);

            modelBuilder.Entity<trMembership>()
                .Property(e => e.BOID)
                .IsUnicode(false);

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
                .HasMany(e => e.strPaymentMembers)
                .WithRequired(e => e.trMembership)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trMembership>()
                .HasMany(e => e.trMembership1)
                .WithOptional(e => e.trMembership2)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<trPaymentWith>()
                .Property(e => e.payAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<trPaymentWith>()
                .HasMany(e => e.strPayments)
                .WithRequired(e => e.trPaymentWith)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPersonalTrainer>()
                .Property(e => e.MemberID)
                .IsUnicode(false);

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
                .Property(e => e.BOID)
                .IsUnicode(false);

            modelBuilder.Entity<trPersonalTrainer>()
                .HasMany(e => e.strKlaimPTs)
                .WithRequired(e => e.trPersonalTrainer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<trPlanAktifitasPT>()
                .Property(e => e.MemberID)
                .IsUnicode(false);

            modelBuilder.Entity<trPlanAktifitasPT>()
                .Property(e => e.BOID)
                .IsUnicode(false);

            modelBuilder.Entity<trPlanKela>()
                .Property(e => e.KelasID)
                .IsUnicode(false);

            modelBuilder.Entity<trPlanKela>()
                .Property(e => e.InstrukurID)
                .IsUnicode(false);

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
                .Property(e => e.BOID)
                .IsUnicode(false);

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
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPersonalTrainers)
                .WithRequired(e => e.tUserBackOffice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPlanAktifitasPTs)
                .WithRequired(e => e.tUserBackOffice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tUserBackOffice>()
                .HasMany(e => e.trPlanKelas)
                .WithRequired(e => e.tUserBackOffice)
                .HasForeignKey(e => e.InstrukurID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.AggreementNo)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.FixAgrrementNo)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.FixType)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.PaymentAdmin)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Payment)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Payment2)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Excel>()
                .Property(e => e.PaymentBy)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.PaymentDesc)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.PaymentNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Telp)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.SalesName)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Excel>()
                .Property(e => e.Membersince)
                .IsUnicode(false);
        }
    }
}

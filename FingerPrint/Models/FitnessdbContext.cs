using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UareUSampleCSharp.Models
{
    public partial class FitnessdbContext : DbContext
    {
        public FitnessdbContext()
        {
        }

        public FitnessdbContext(DbContextOptions<FitnessdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationGroupRoles> ApplicationGroupRoles { get; set; }
        public virtual DbSet<ApplicationGroups> ApplicationGroups { get; set; }
        public virtual DbSet<ApplicationUserGroups> ApplicationUserGroups { get; set; }
        public virtual DbSet<AspForm> AspForm { get; set; }
        public virtual DbSet<AspFormAuthorization> AspFormAuthorization { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ExcelLipo5> ExcelLipo5 { get; set; }
        public virtual DbSet<ExcelPt> ExcelPt { get; set; }
        public virtual DbSet<ExcelUploadDb> ExcelUploadDb { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Logtest> Logtest { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<StrActionKlaim> StrActionKlaim { get; set; }
        public virtual DbSet<StrActionKlaimParam> StrActionKlaimParam { get; set; }
        public virtual DbSet<StrAktivitasSales> StrAktivitasSales { get; set; }
        public virtual DbSet<StrKlaimPt> StrKlaimPt { get; set; }
        public virtual DbSet<StrLocBo> StrLocBo { get; set; }
        public virtual DbSet<StrLocMember> StrLocMember { get; set; }
        public virtual DbSet<StrPayment> StrPayment { get; set; }
        public virtual DbSet<StrPaymentMember> StrPaymentMember { get; set; }
        public virtual DbSet<StrPersonEvent> StrPersonEvent { get; set; }
        public virtual DbSet<TActionPt> TActionPt { get; set; }
        public virtual DbSet<TBank> TBank { get; set; }
        public virtual DbSet<TCardStatus> TCardStatus { get; set; }
        public virtual DbSet<TEvent> TEvent { get; set; }
        public virtual DbSet<TEventScore> TEventScore { get; set; }
        public virtual DbSet<TEventStep> TEventStep { get; set; }
        public virtual DbSet<TKelas> TKelas { get; set; }
        public virtual DbSet<TLocFitnessCenter> TLocFitnessCenter { get; set; }
        public virtual DbSet<TMember> TMember { get; set; }
        public virtual DbSet<TMemberState> TMemberState { get; set; }
        public virtual DbSet<TMemberType> TMemberType { get; set; }
        public virtual DbSet<TPaketPt> TPaketPt { get; set; }
        public virtual DbSet<TPaymentType> TPaymentType { get; set; }
        public virtual DbSet<TPerson> TPerson { get; set; }
        public virtual DbSet<TPosisi> TPosisi { get; set; }
        public virtual DbSet<TRoleEvent> TRoleEvent { get; set; }
        public virtual DbSet<TRuangKelas> TRuangKelas { get; set; }
        public virtual DbSet<TSalesAction> TSalesAction { get; set; }
        public virtual DbSet<TStatusMember> TStatusMember { get; set; }
        public virtual DbSet<TStatusMemberPrice> TStatusMemberPrice { get; set; }
        public virtual DbSet<TTypeStatusCinCout> TTypeStatusCinCout { get; set; }
        public virtual DbSet<TUserBackOffice> TUserBackOffice { get; set; }
        public virtual DbSet<TrAktifitasKelas> TrAktifitasKelas { get; set; }
        public virtual DbSet<TrAktifitasMember> TrAktifitasMember { get; set; }
        public virtual DbSet<TrCinCout> TrCinCout { get; set; }
        public virtual DbSet<TrEventScore> TrEventScore { get; set; }
        public virtual DbSet<TrEventStep> TrEventStep { get; set; }
        public virtual DbSet<TrMembership> TrMembership { get; set; }
        public virtual DbSet<TrPaymentWith> TrPaymentWith { get; set; }
        public virtual DbSet<TrPembayaran> TrPembayaran { get; set; }
        public virtual DbSet<TrPersonEvent> TrPersonEvent { get; set; }
        public virtual DbSet<TrPersonalTrainer> TrPersonalTrainer { get; set; }
        public virtual DbSet<TrPlanAktifitasPt> TrPlanAktifitasPt { get; set; }
        public virtual DbSet<TrPlanKelas> TrPlanKelas { get; set; }
        public virtual DbSet<TrTtd> TrTtd { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=fitnessDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<ApplicationGroupRoles>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationRoleId, e.ApplicationGroupId })
                    .HasName("PK_dbo.ApplicationGroupRoles");

                entity.Property(e => e.ApplicationRoleId).HasMaxLength(128);

                entity.Property(e => e.ApplicationGroupId).HasMaxLength(128);

                entity.HasOne(d => d.ApplicationGroup)
                    .WithMany(p => p.ApplicationGroupRoles)
                    .HasForeignKey(d => d.ApplicationGroupId)
                    .HasConstraintName("FK_dbo.ApplicationGroupRoles_dbo.ApplicationGroups_ApplicationGroupId");
            });

            modelBuilder.Entity<ApplicationGroups>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<ApplicationUserGroups>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationUserId, e.ApplicationGroupId })
                    .HasName("PK_dbo.ApplicationUserGroups");

                entity.Property(e => e.ApplicationUserId).HasMaxLength(128);

                entity.Property(e => e.ApplicationGroupId).HasMaxLength(128);

                entity.HasOne(d => d.ApplicationGroup)
                    .WithMany(p => p.ApplicationUserGroups)
                    .HasForeignKey(d => d.ApplicationGroupId)
                    .HasConstraintName("FK_dbo.ApplicationUserGroups_dbo.ApplicationGroups_ApplicationGroupId");
            });

            modelBuilder.Entity<AspForm>(entity =>
            {
                entity.HasKey(e => e.FormId)
                    .HasName("PK_node");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Area)
                    .HasColumnName("area")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Clickable).HasColumnName("clickable");

                entity.Property(e => e.Controller)
                    .HasColumnName("controller")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Desciption)
                    .HasColumnName("desciption")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

                entity.Property(e => e.Iconclass)
                    .HasColumnName("iconclass")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MasterModule)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Module)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Parameter)
                    .HasColumnName("parameter")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("parent_ID");

                entity.Property(e => e.StatusCode)
                    .HasColumnName("statusCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Visible).HasColumnName("visible");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_AspForm_AspForm");
            });

            modelBuilder.Entity<AspFormAuthorization>(entity =>
            {
                entity.HasKey(e => e.AuthId);

                entity.Property(e => e.AuthId).HasColumnName("authID");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.AspFormAuthorization)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspFormAuthorization_AspForm");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AspFormAuthorization)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspFormAuthorization_ApplicationGroups1");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ExcelLipo5>(entity =>
            {
                entity.HasKey(e => new { e.No, e.Agreement });

                entity.Property(e => e.Agreement)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AgreementFix)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipFix)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipPrFix)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipType)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExcelPt>(entity =>
            {
                entity.HasKey(e => e.No);

                entity.ToTable("ExcelPT");

                entity.Property(e => e.No).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.Aggrement).HasMaxLength(50);

                entity.Property(e => e.By1).HasMaxLength(50);

                entity.Property(e => e.By2).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Efd)
                    .HasColumnName("EFD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Exp)
                    .HasColumnName("EXP")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nama).HasMaxLength(50);

                entity.Property(e => e.Paid).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Paid2).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.Telp).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<ExcelUploadDb>(entity =>
            {
                entity.HasKey(e => e.No);

                entity.ToTable("ExcelUploadDB");

                entity.Property(e => e.No).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Agreement)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AgreementFix)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bank)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.By1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.By2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Efd)
                    .HasColumnName("EFD")
                    .HasColumnType("date");

                entity.Property(e => e.Exp)
                    .HasColumnName("EXP")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mbr1)
                    .HasColumnName("MBR1")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Mbr2)
                    .HasColumnName("MBR2")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MemberName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipType)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MembershipTypeFix)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Paid1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Paid2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Paid3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Payment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Prfix)
                    .HasColumnName("prfix")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RecordType)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Sold)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telp)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("log");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Exception)
                    .HasColumnName("exception")
                    .IsUnicode(false);

                entity.Property(e => e.IdTable)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Logger)
                    .HasColumnName("logger")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .IsUnicode(false);

                entity.Property(e => e.NamaTable)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NamaTransaksi)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Utcdate)
                    .HasColumnName("utcdate")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Logtest>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Exception)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Logger)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Thread)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<StrActionKlaim>(entity =>
            {
                entity.HasKey(e => e.StrActionClaimId)
                    .HasName("PK_dbo.StrActionKlaim");

                entity.Property(e => e.StrActionClaimId).HasColumnName("strActionClaimID");

                entity.Property(e => e.ActionPtid).HasColumnName("ActionPTID");

                entity.Property(e => e.ClaimId).HasColumnName("ClaimID");

                entity.HasOne(d => d.ActionPt)
                    .WithMany(p => p.StrActionKlaim)
                    .HasForeignKey(d => d.ActionPtid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StrActionKlaim_tActionPT");
            });

            modelBuilder.Entity<StrActionKlaimParam>(entity =>
            {
                entity.HasKey(e => e.IdPar)
                    .HasName("PK_dbo.ActionKlaimParam");

                entity.Property(e => e.NamaParam)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Satuan)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.StrActionClaim)
                    .WithMany(p => p.StrActionKlaimParam)
                    .HasForeignKey(d => d.StrActionClaimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StrActionKlaimParam_StrActionKlaim");
            });

            modelBuilder.Entity<StrAktivitasSales>(entity =>
            {
                entity.HasKey(e => e.AktivitasSalesId)
                    .HasName("PK_trAktivitasSales");

                entity.ToTable("strAktivitasSales");

                entity.Property(e => e.AktivitasSalesId).HasColumnName("AktivitasSalesID");

                entity.Property(e => e.Aktivitasstap)
                    .HasColumnName("aktivitasstap")
                    .IsRowVersion();

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.MemberStateId).HasColumnName("MemberStateID");

                entity.Property(e => e.SalesActionId).HasColumnName("SalesActionID");

                entity.Property(e => e.TrMembershipId).HasColumnName("trMembershipID");

                entity.HasOne(d => d.MemberState)
                    .WithMany(p => p.StrAktivitasSales)
                    .HasForeignKey(d => d.MemberStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktivitasSales_tMemberState");

                entity.HasOne(d => d.SalesAction)
                    .WithMany(p => p.StrAktivitasSales)
                    .HasForeignKey(d => d.SalesActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktivitasSales_tSalesAction");

                entity.HasOne(d => d.TrMembership)
                    .WithMany(p => p.StrAktivitasSales)
                    .HasForeignKey(d => d.TrMembershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktivitasSales_trMembership");
            });

            modelBuilder.Entity<StrKlaimPt>(entity =>
            {
                entity.HasKey(e => e.ClaimPtid);

                entity.ToTable("strKlaimPT");

                entity.Property(e => e.ClaimPtid).HasColumnName("ClaimPTID");

                entity.Property(e => e.AkhirClaim).HasColumnType("datetime");

                entity.Property(e => e.AktifitasMemberId).HasColumnName("AktifitasMemberID");

                entity.Property(e => e.AwalClaim).HasColumnType("datetime");

                entity.Property(e => e.TrPersonalTrainerId).HasColumnName("trPersonalTrainerID");

                entity.Property(e => e.VerifikasiMember)
                    .IsRequired()
                    .HasColumnName("verifikasiMember")
                    .HasMaxLength(50);

                entity.Property(e => e.VerifikasiPt)
                    .IsRequired()
                    .HasColumnName("verifikasiPT")
                    .HasMaxLength(50);

                entity.HasOne(d => d.AktifitasMember)
                    .WithMany(p => p.StrKlaimPt)
                    .HasForeignKey(d => d.AktifitasMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strKlaimPT_trAktifitasMember");

                entity.HasOne(d => d.TrPersonalTrainer)
                    .WithMany(p => p.StrKlaimPt)
                    .HasForeignKey(d => d.TrPersonalTrainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strKlaimPT_trPersonalTrainer");
            });

            modelBuilder.Entity<StrLocBo>(entity =>
            {
                entity.HasKey(e => e.LocBoId);

                entity.ToTable("strLocBO");

                entity.Property(e => e.LocBoId).HasColumnName("LocBoID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.PersonBoid).HasColumnName("PersonBOID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.StrLocBo)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strLocBO_tLocFitnessCenter");

                entity.HasOne(d => d.PersonBo)
                    .WithMany(p => p.StrLocBo)
                    .HasForeignKey(d => d.PersonBoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strLocBO_tUserBackOffice1");
            });

            modelBuilder.Entity<StrLocMember>(entity =>
            {
                entity.HasKey(e => e.LocMemberId)
                    .HasName("PK_strLocMember_1");

                entity.ToTable("strLocMember");

                entity.Property(e => e.LocMemberId).HasColumnName("LocMemberID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.StrLocMember)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strLocMember_tLocFitnessCenter");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.StrLocMember)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strLocMember_tMember1");
            });

            modelBuilder.Entity<StrPayment>(entity =>
            {
                entity.ToTable("strPayment");

                entity.Property(e => e.StrPaymentId).HasColumnName("StrPaymentID");

                entity.Property(e => e.PaymentWithId).HasColumnName("PaymentWithID");

                entity.Property(e => e.TrPaymentId).HasColumnName("trPaymentID");

                entity.HasOne(d => d.PaymentWith)
                    .WithMany(p => p.StrPayment)
                    .HasForeignKey(d => d.PaymentWithId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strPayment_trPaymentWith");

                entity.HasOne(d => d.TrPayment)
                    .WithMany(p => p.StrPayment)
                    .HasForeignKey(d => d.TrPaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strPayment_strPaymentMember");
            });

            modelBuilder.Entity<StrPaymentMember>(entity =>
            {
                entity.HasKey(e => e.TrPaymentId)
                    .HasName("PK_trPayment");

                entity.ToTable("strPaymentMember");

                entity.Property(e => e.TrPaymentId).HasColumnName("trPaymentID");

                entity.Property(e => e.MemberTypeId).HasColumnName("MemberTypeID");

                entity.Property(e => e.MembershipDtlid).HasColumnName("MembershipDTLID");

                entity.Property(e => e.Pembayaranke).HasColumnName("pembayaranke");

                entity.Property(e => e.StatusBayar).HasColumnName("statusBayar");

                entity.Property(e => e.Tanggal).HasColumnType("date");

                entity.Property(e => e.TimeStamp).IsRowVersion();

                entity.Property(e => e.TrMembershipId).HasColumnName("trMembershipID");

                entity.HasOne(d => d.MemberType)
                    .WithMany(p => p.StrPaymentMember)
                    .HasForeignKey(d => d.MemberTypeId)
                    .HasConstraintName("FK_strPayment_tMemberType");

                entity.HasOne(d => d.MembershipDtl)
                    .WithMany(p => p.StrPaymentMemberMembershipDtl)
                    .HasForeignKey(d => d.MembershipDtlid)
                    .HasConstraintName("FK_strPaymentMember_trMembership");

                entity.HasOne(d => d.TrMembership)
                    .WithMany(p => p.StrPaymentMemberTrMembership)
                    .HasForeignKey(d => d.TrMembershipId)
                    .HasConstraintName("FK_strPayment_trMembership");
            });

            modelBuilder.Entity<StrPersonEvent>(entity =>
            {
                entity.HasKey(e => e.PersonEventId)
                    .HasName("PK_strPersonEvent_1");

                entity.ToTable("strPersonEvent");

                entity.Property(e => e.PersonEventId).ValueGeneratedNever();

                entity.Property(e => e.Gym)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PersonEvent)
                    .WithOne(p => p.StrPersonEvent)
                    .HasForeignKey<StrPersonEvent>(d => d.PersonEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_strPersonEvent_trPersonEvent1");
            });

            modelBuilder.Entity<TActionPt>(entity =>
            {
                entity.HasKey(e => e.ActionPtid);

                entity.ToTable("tActionPT");

                entity.Property(e => e.ActionPtid).HasColumnName("ActionPTID");

                entity.Property(e => e.ActionPtket).HasColumnName("ActionPTKet");

                entity.Property(e => e.ActionPtname)
                    .IsRequired()
                    .HasColumnName("ActionPTName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TBank>(entity =>
            {
                entity.HasKey(e => e.BankId);

                entity.ToTable("tBank");

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.Edc).HasColumnName("EDC");

                entity.Property(e => e.NamaBank).HasMaxLength(50);
            });

            modelBuilder.Entity<TCardStatus>(entity =>
            {
                entity.HasKey(e => e.CardStatusId);

                entity.ToTable("tCardStatus");

                entity.Property(e => e.CardStatusId).HasColumnName("CardStatusID");

                entity.Property(e => e.CardDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardStatus)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TEvent>(entity =>
            {
                entity.HasKey(e => e.EvId);

                entity.ToTable("tEvent");

                entity.Property(e => e.EvEndDate).HasColumnType("date");

                entity.Property(e => e.EvName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EvStartDate).HasColumnType("date");
            });

            modelBuilder.Entity<TEventScore>(entity =>
            {
                entity.HasKey(e => e.EvScoreId)
                    .HasName("PK_tEventScore_1");

                entity.ToTable("tEventScore");

                entity.Property(e => e.EvScoreName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EvStep)
                    .WithMany(p => p.TEventScore)
                    .HasForeignKey(d => d.EvStepId)
                    .HasConstraintName("FK_tEventScore_tEventStep");
            });

            modelBuilder.Entity<TEventStep>(entity =>
            {
                entity.HasKey(e => e.EvStepId)
                    .HasName("PK_tEventStep_1");

                entity.ToTable("tEventStep");

                entity.Property(e => e.StepDetail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.TEventStep)
                    .HasForeignKey(d => d.EvId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tEventStep_tEvent");
            });

            modelBuilder.Entity<TKelas>(entity =>
            {
                entity.HasKey(e => e.KelasId);

                entity.ToTable("tKelas");

                entity.Property(e => e.KelasId)
                    .HasColumnName("KelasID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ImageKelas).HasColumnType("image");

                entity.Property(e => e.KnamaKelas)
                    .IsRequired()
                    .HasColumnName("KNamaKelas")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RuangKelasId).HasColumnName("RuangKelasID");

                entity.HasOne(d => d.RuangKelas)
                    .WithMany(p => p.TKelas)
                    .HasForeignKey(d => d.RuangKelasId)
                    .HasConstraintName("FK_tKelas_tRuangKelas");
            });

            modelBuilder.Entity<TLocFitnessCenter>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("tLocFitnessCenter");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Lalamat)
                    .IsRequired()
                    .HasColumnName("LAlamat")
                    .HasMaxLength(50);

                entity.Property(e => e.Lauth)
                    .IsRequired()
                    .HasColumnName("LAuth")
                    .HasMaxLength(50);

                entity.Property(e => e.Lfax)
                    .IsRequired()
                    .HasColumnName("LFax")
                    .HasMaxLength(50);

                entity.Property(e => e.Ltlp)
                    .IsRequired()
                    .HasColumnName("LTlp")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TMember>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK_tMember_1");

                entity.ToTable("tMember");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MemberNo)
                    .HasColumnName("MemberNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemberTypeId).HasColumnName("MemberTypeID");

                entity.Property(e => e.Mfoto)
                    .HasColumnName("MFoto")
                    .HasColumnType("text");

                entity.Property(e => e.MfotoCcdebit)
                    .HasColumnName("MFotoCCDebit")
                    .HasColumnType("text");

                entity.Property(e => e.MfotoKtp)
                    .HasColumnName("MFotoKTP")
                    .HasColumnType("text");

                entity.Property(e => e.MfotoSignature)
                    .HasColumnName("MFotoSignature")
                    .HasColumnType("text");

                entity.Property(e => e.MfotoUrl)
                    .HasColumnName("MFotoUrl")
                    .HasMaxLength(50);

                entity.Property(e => e.Mrfid).HasColumnName("MRFID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.HasOne(d => d.MemberType)
                    .WithMany(p => p.TMember)
                    .HasForeignKey(d => d.MemberTypeId)
                    .HasConstraintName("FK_tMember_tMemberType");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.TMember)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tMember_tPerson");
            });

            modelBuilder.Entity<TMemberState>(entity =>
            {
                entity.HasKey(e => e.MemberStateId);

                entity.ToTable("tMemberState");

                entity.Property(e => e.MemberStateId).HasColumnName("MemberStateID");

                entity.Property(e => e.MemberStateName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TMemberType>(entity =>
            {
                entity.HasKey(e => e.MemberTypeId);

                entity.ToTable("tMemberType");

                entity.Property(e => e.MemberTypeId).HasColumnName("MemberTypeID");

                entity.Property(e => e.Admin)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Biaya).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MemberType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Periode)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.PrFix)
                    .IsRequired()
                    .HasColumnName("prFix")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Prorate)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShareMin).HasDefaultValueSql("((0))");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TPaketPersonalTrainerId).HasColumnName("tPaketPersonalTrainerID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TMemberType)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_tMemberType_tLocFitnessCenter");

                entity.HasOne(d => d.TPaketPersonalTrainer)
                    .WithMany(p => p.TMemberType)
                    .HasForeignKey(d => d.TPaketPersonalTrainerId)
                    .HasConstraintName("FK_tMemberType_tPaketPT");
            });

            modelBuilder.Entity<TPaketPt>(entity =>
            {
                entity.ToTable("tPaketPT");

                entity.Property(e => e.TPaketPtid).HasColumnName("tPaketPTID");

                entity.Property(e => e.Pptdesc)
                    .IsRequired()
                    .HasColumnName("PPTDesc")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Pptharga)
                    .HasColumnName("PPTHarga")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Pptjam).HasColumnName("PPTJam");

                entity.Property(e => e.Pptmasa).HasColumnName("PPTMasa");

                entity.Property(e => e.Pptmembership).HasColumnName("PPTMembership");

                entity.Property(e => e.Pptnama)
                    .IsRequired()
                    .HasColumnName("PPTNama")
                    .HasMaxLength(50);

                entity.Property(e => e.PptpersonTotal).HasColumnName("PPTPersonTotal");

                entity.Property(e => e.Pptstatus)
                    .IsRequired()
                    .HasColumnName("PPTStatus")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TPaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentTypeId)
                    .HasName("PK_tJenisKartu");

                entity.ToTable("tPaymentType");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.NamaType).HasMaxLength(50);
            });

            modelBuilder.Entity<TPerson>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("tPerson");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Palamat)
                    .HasColumnName("PAlamat")
                    .HasMaxLength(200);

                entity.Property(e => e.Pemail)
                    .HasColumnName("PEmail")
                    .HasMaxLength(50);

                entity.Property(e => e.Pgender)
                    .HasColumnName("PGender")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Php1)
                    .HasColumnName("PHP1")
                    .HasMaxLength(50);

                entity.Property(e => e.Php2)
                    .HasColumnName("PHP2")
                    .HasMaxLength(15);

                entity.Property(e => e.Pidentitas)
                    .HasColumnName("PIdentitas")
                    .HasMaxLength(50);

                entity.Property(e => e.Pkecamatan)
                    .HasColumnName("PKecamatan")
                    .HasMaxLength(50);

                entity.Property(e => e.Pkelurahan)
                    .HasColumnName("PKelurahan")
                    .HasMaxLength(50);

                entity.Property(e => e.Pkota)
                    .HasColumnName("PKota")
                    .HasMaxLength(50);

                entity.Property(e => e.Pnama)
                    .IsRequired()
                    .HasColumnName("PNama")
                    .HasMaxLength(50);

                entity.Property(e => e.PpinBb)
                    .HasColumnName("PPinBB")
                    .HasMaxLength(10);

                entity.Property(e => e.Ppropinsi)
                    .HasColumnName("PPropinsi")
                    .HasMaxLength(50);

                entity.Property(e => e.Prt)
                    .HasColumnName("PRT")
                    .HasMaxLength(3);

                entity.Property(e => e.Prw)
                    .HasColumnName("PRW")
                    .HasMaxLength(3);

                entity.Property(e => e.Ptelp)
                    .HasColumnName("PTelp")
                    .HasMaxLength(50);

                entity.Property(e => e.PtempLahir)
                    .HasColumnName("PTempLahir")
                    .HasMaxLength(50);

                entity.Property(e => e.PtglLahir)
                    .HasColumnName("PTglLahir")
                    .HasColumnType("date");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TPerson)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_tPerson_AspNetUsers");
            });

            modelBuilder.Entity<TPosisi>(entity =>
            {
                entity.HasKey(e => e.PosisiId);

                entity.ToTable("tPosisi");

                entity.Property(e => e.PosisiId).HasColumnName("PosisiID");

                entity.Property(e => e.PnamaPosisi)
                    .IsRequired()
                    .HasColumnName("PNamaPosisi")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TRoleEvent>(entity =>
            {
                entity.HasKey(e => e.EvRoleId);

                entity.ToTable("tRoleEvent");

                entity.Property(e => e.EvRoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TRuangKelas>(entity =>
            {
                entity.HasKey(e => e.RuangKelasId);

                entity.ToTable("tRuangKelas");

                entity.Property(e => e.RuangKelasId).HasColumnName("RuangKelasID");

                entity.Property(e => e.BackGround).HasMaxLength(50);

                entity.Property(e => e.NruangKelas)
                    .HasColumnName("NRuangKelas")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSalesAction>(entity =>
            {
                entity.HasKey(e => e.SalesActionId);

                entity.ToTable("tSalesAction");

                entity.Property(e => e.SalesActionId).HasColumnName("SalesActionID");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TStatusMember>(entity =>
            {
                entity.HasKey(e => e.StatusMid);

                entity.ToTable("tStatusMember");

                entity.Property(e => e.StatusMid).HasColumnName("StatusMID");

                entity.Property(e => e.Stket)
                    .IsRequired()
                    .HasColumnName("STKet")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TStatusMemberPrice>(entity =>
            {
                entity.ToTable("tStatusMemberPrice");

                entity.Property(e => e.TStatusMemberPriceId).HasColumnName("tStatusMemberPriceId");

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StatusMid).HasColumnName("StatusMID");

                entity.HasOne(d => d.StatusM)
                    .WithMany(p => p.TStatusMemberPrice)
                    .HasForeignKey(d => d.StatusMid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tStatusMemberPrice_tStatusMember1");
            });

            modelBuilder.Entity<TTypeStatusCinCout>(entity =>
            {
                entity.HasKey(e => e.TypeStatusInOut);

                entity.ToTable("tTypeStatusCinCout");

                entity.Property(e => e.NameStatusInOut)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TUserBackOffice>(entity =>
            {
                entity.HasKey(e => e.PersonBoid);

                entity.ToTable("tUserBackOffice");

                entity.Property(e => e.PersonBoid)
                    .HasColumnName("PersonBOID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Background)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bfoto)
                    .HasColumnName("BFoto")
                    .HasColumnType("image");

                entity.Property(e => e.BfotoKtp)
                    .HasColumnName("BFotoKTP")
                    .HasColumnType("image");

                entity.Property(e => e.BfotoSignature)
                    .HasColumnName("BFotoSignature")
                    .HasColumnType("image");

                entity.Property(e => e.Bmulai)
                    .HasColumnName("BMulai")
                    .HasColumnType("date");

                entity.Property(e => e.Boidno)
                    .IsRequired()
                    .HasColumnName("BOIDNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Brfid).HasColumnName("BRFID");

                entity.Property(e => e.PosisiId).HasColumnName("PosisiID");

                entity.Property(e => e.StatusBoid).HasColumnName("StatusBOID");

                entity.HasOne(d => d.PersonBo)
                    .WithOne(p => p.TUserBackOffice)
                    .HasForeignKey<TUserBackOffice>(d => d.PersonBoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tUserBackOffice_tPerson");

                entity.HasOne(d => d.Posisi)
                    .WithMany(p => p.TUserBackOffice)
                    .HasForeignKey(d => d.PosisiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tUserBackOffice_tPosisi");
            });

            modelBuilder.Entity<TrAktifitasKelas>(entity =>
            {
                entity.HasKey(e => e.AktifitasKelasId);

                entity.ToTable("trAktifitasKelas");

                entity.Property(e => e.AktifitasKelasId).HasColumnName("AktifitasKelasID");

                entity.Property(e => e.AktifitasMemberId).HasColumnName("AktifitasMemberID");

                entity.Property(e => e.PlanKelasId).HasColumnName("PlanKelasID");

                entity.HasOne(d => d.AktifitasMember)
                    .WithMany(p => p.TrAktifitasKelas)
                    .HasForeignKey(d => d.AktifitasMemberId)
                    .HasConstraintName("FK_trAktifitasKelas_trAktifitasMember");

                entity.HasOne(d => d.PlanKelas)
                    .WithMany(p => p.TrAktifitasKelas)
                    .HasForeignKey(d => d.PlanKelasId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktifitasKelas_trPlanKelas");
            });

            modelBuilder.Entity<TrAktifitasMember>(entity =>
            {
                entity.HasKey(e => e.AktifitasMemberId);

                entity.ToTable("trAktifitasMember");

                entity.Property(e => e.AktifitasMemberId).HasColumnName("AktifitasMemberID");

                entity.Property(e => e.ActionPtid).HasColumnName("ActionPTID");

                entity.Property(e => e.Ammulai)
                    .HasColumnName("AMMulai")
                    .HasColumnType("datetime");

                entity.Property(e => e.Amselesai)
                    .HasColumnName("AMSelesai")
                    .HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PersonBoid).HasColumnName("PersonBOID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.VerifikasiMember)
                    .HasColumnName("verifikasiMember")
                    .HasMaxLength(50);

                entity.Property(e => e.VerifikasiToken)
                    .HasColumnName("verifikasiToken")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActionPt)
                    .WithMany(p => p.TrAktifitasMember)
                    .HasForeignKey(d => d.ActionPtid)
                    .HasConstraintName("FK_trAktifitasMember_tActionPT");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TrAktifitasMember)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktifitasMember_tLocFitnessCenter");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TrAktifitasMember)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktifitasMember_tMember");

                entity.HasOne(d => d.PersonBo)
                    .WithMany(p => p.TrAktifitasMember)
                    .HasForeignKey(d => d.PersonBoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trAktifitasMember_tUserBackOffice1");
            });

            modelBuilder.Entity<TrCinCout>(entity =>
            {
                entity.HasKey(e => e.CinCoutId);

                entity.ToTable("trCinCout");

                entity.Property(e => e.CinCoutId).HasColumnName("CinCoutID");

                entity.Property(e => e.LocBoId).HasColumnName("LocBoID");

                entity.Property(e => e.PersonBoid).HasColumnName("PersonBOID");

                entity.Property(e => e.RefCinCout)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.TimeStatus).HasColumnType("datetime");

                entity.HasOne(d => d.LocBo)
                    .WithMany(p => p.TrCinCout)
                    .HasForeignKey(d => d.LocBoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trCinCout_strLocBO");

                entity.HasOne(d => d.PersonBo)
                    .WithMany(p => p.TrCinCout)
                    .HasForeignKey(d => d.PersonBoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trCinCout_tUserBackOffice1");

                entity.HasOne(d => d.TypeStatusInOutNavigation)
                    .WithMany(p => p.TrCinCout)
                    .HasForeignKey(d => d.TypeStatusInOut)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trCinCout_tTypeStatusCinCout");
            });

            modelBuilder.Entity<TrEventScore>(entity =>
            {
                entity.HasKey(e => e.TrEvScoreId);

                entity.ToTable("trEventScore");

                entity.Property(e => e.TrEvScoreId).HasColumnName("trEvScoreId");

                entity.Property(e => e.TrEventStepId).HasColumnName("trEventStepId");

                entity.HasOne(d => d.EvScore)
                    .WithMany(p => p.TrEventScore)
                    .HasForeignKey(d => d.EvScoreId)
                    .HasConstraintName("FK_trEventScore_tEventScore");

                entity.HasOne(d => d.PersonEvent)
                    .WithMany(p => p.TrEventScore)
                    .HasForeignKey(d => d.PersonEventId)
                    .HasConstraintName("FK_trEventScore_trPersonEvent");

                entity.HasOne(d => d.TrEventStep)
                    .WithMany(p => p.TrEventScore)
                    .HasForeignKey(d => d.TrEventStepId)
                    .HasConstraintName("FK_trEventScore_trEventStep");
            });

            modelBuilder.Entity<TrEventStep>(entity =>
            {
                entity.ToTable("trEventStep");

                entity.Property(e => e.TrEventStepId).HasColumnName("trEventStepId");

                entity.HasOne(d => d.EvStep)
                    .WithMany(p => p.TrEventStep)
                    .HasForeignKey(d => d.EvStepId)
                    .HasConstraintName("FK_trEventStep_tEventStep1");

                entity.HasOne(d => d.PersonEvent)
                    .WithMany(p => p.TrEventStep)
                    .HasForeignKey(d => d.PersonEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trEventStep_trPersonEvent");
            });

            modelBuilder.Entity<TrMembership>(entity =>
            {
                entity.ToTable("trMembership");

                entity.HasIndex(e => e.AgreementId)
                    .HasName("IX_trMembership_1");

                entity.Property(e => e.TrMembershipId).HasColumnName("trMembershipID");

                entity.Property(e => e.AccountingStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ActivationCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Admin)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AgreementId)
                    .HasColumnName("AgreementID")
                    .HasMaxLength(50);

                entity.Property(e => e.CountMember).HasDefaultValueSql("((1))");

                entity.Property(e => e.Disc).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiscType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscVal).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Msinput)
                    .HasColumnName("MSInput")
                    .HasColumnType("datetime");

                entity.Property(e => e.MstglMulai)
                    .HasColumnName("MSTglMulai")
                    .HasColumnType("date");

                entity.Property(e => e.MstglSelesai)
                    .HasColumnName("MSTglSelesai")
                    .HasColumnType("date");

                entity.Property(e => e.Mstime)
                    .IsRequired()
                    .HasColumnName("MSTime")
                    .IsRowVersion();

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.PersonBoidadm).HasColumnName("PersonBOIDADM");

                entity.Property(e => e.PersonBoidsales).HasColumnName("PersonBOIDSales");

                entity.Property(e => e.Prorate)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Seq).HasColumnName("seq");

                entity.Property(e => e.StatusMid).HasColumnName("StatusMID");

                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CardStatusNavigation)
                    .WithMany(p => p.TrMembership)
                    .HasForeignKey(d => d.CardStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trMembership_tCardStatus");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TrMembership)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trMembership_tLocFitnessCenter");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TrMembership)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trMembership_tMember");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_trMembership_trMembership");

                entity.HasOne(d => d.PersonBoidadmNavigation)
                    .WithMany(p => p.TrMembershipPersonBoidadmNavigation)
                    .HasForeignKey(d => d.PersonBoidadm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trMembership_tUserBackOffice2");

                entity.HasOne(d => d.PersonBoidsalesNavigation)
                    .WithMany(p => p.TrMembershipPersonBoidsalesNavigation)
                    .HasForeignKey(d => d.PersonBoidsales)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trMembership_tUserBackOffice3");

                entity.HasOne(d => d.StatusM)
                    .WithMany(p => p.TrMembership)
                    .HasForeignKey(d => d.StatusMid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trMembership_tStatusMember");
            });

            modelBuilder.Entity<TrPaymentWith>(entity =>
            {
                entity.HasKey(e => e.PaymentWithId);

                entity.ToTable("trPaymentWith");

                entity.Property(e => e.PaymentWithId).HasColumnName("PaymentWithID");

                entity.Property(e => e.ApprCode).HasMaxLength(50);

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.Edcid).HasColumnName("EDCID");

                entity.Property(e => e.Mbramount)
                    .HasColumnName("MBRAmount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NoKartu).HasMaxLength(50);

                entity.Property(e => e.PaidAmount)
                    .HasColumnName("paidAmount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PayAmount)
                    .HasColumnName("payAmount")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.Property(e => e.Pemegang).HasMaxLength(50);

                entity.Property(e => e.TraceCode).HasMaxLength(50);

                entity.Property(e => e.ValidUntil).HasColumnType("date");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.TrPaymentWithBank)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_trPaymentWith_tBank");

                entity.HasOne(d => d.Edc)
                    .WithMany(p => p.TrPaymentWithEdc)
                    .HasForeignKey(d => d.Edcid)
                    .HasConstraintName("FK_trPaymentWith_tBank1");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.TrPaymentWith)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPaymentWith_tJenisKartu");
            });

            modelBuilder.Entity<TrPembayaran>(entity =>
            {
                entity.HasKey(e => e.Idpembayaran);

                entity.ToTable("trPembayaran");

                entity.Property(e => e.Idpembayaran).HasColumnName("IDPEMBAYARAN");

                entity.Property(e => e.Cash).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Cc)
                    .HasColumnName("CC")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DJumlah)
                    .HasColumnName("D_JUMLAH")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DTgl)
                    .HasColumnName("D_TGL")
                    .HasColumnType("datetime");

                entity.Property(e => e.Debit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdTransaksi).HasColumnName("ID_Transaksi");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PersonBoidfin).HasColumnName("PersonBOIDFin");

                entity.Property(e => e.TransaksiTipe).HasMaxLength(50);

                entity.Property(e => e.Ts)
                    .IsRequired()
                    .HasColumnName("TS")
                    .IsRowVersion();

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TrPembayaran)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_trPembayaran_tMember1");

                entity.HasOne(d => d.PersonBoidfinNavigation)
                    .WithMany(p => p.TrPembayaran)
                    .HasForeignKey(d => d.PersonBoidfin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPembayaran_tUserBackOffice1");
            });

            modelBuilder.Entity<TrPersonEvent>(entity =>
            {
                entity.HasKey(e => e.PersonEventId);

                entity.ToTable("trPersonEvent");

                entity.HasOne(d => d.Ev)
                    .WithMany(p => p.TrPersonEvent)
                    .HasForeignKey(d => d.EvId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPersonEvent_tEvent");

                entity.HasOne(d => d.EvRole)
                    .WithMany(p => p.TrPersonEvent)
                    .HasForeignKey(d => d.EvRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPersonEvent_tRoleEvent");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.TrPersonEvent)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPersonEvent_tPerson");
            });

            modelBuilder.Entity<TrPersonalTrainer>(entity =>
            {
                entity.ToTable("trPersonalTrainer");

                entity.Property(e => e.TrPersonalTrainerId).HasColumnName("trPersonalTrainerID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.PersonBoidpt).HasColumnName("PersonBOIDPT");

                entity.Property(e => e.Seq).HasColumnName("seq");

                entity.Property(e => e.TPaketPtid).HasColumnName("tPaketPTID");

                entity.Property(e => e.TrMembershipId).HasColumnName("trMembershipID");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_trPersonalTrainer_trPersonalTrainer");

                entity.HasOne(d => d.PersonBoidptNavigation)
                    .WithMany(p => p.TrPersonalTrainer)
                    .HasForeignKey(d => d.PersonBoidpt)
                    .HasConstraintName("FK_trPersonalTrainer_tUserBackOffice");

                entity.HasOne(d => d.TPaketPt)
                    .WithMany(p => p.TrPersonalTrainer)
                    .HasForeignKey(d => d.TPaketPtid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPersonalTrainer_tPaketPT");

                entity.HasOne(d => d.TrMembership)
                    .WithMany(p => p.TrPersonalTrainer)
                    .HasForeignKey(d => d.TrMembershipId)
                    .HasConstraintName("FK_trPersonalTrainer_trMembership1");
            });

            modelBuilder.Entity<TrPlanAktifitasPt>(entity =>
            {
                entity.HasKey(e => e.PlanAktifitasPtid);

                entity.ToTable("trPlanAktifitasPT");

                entity.Property(e => e.PlanAktifitasPtid).HasColumnName("PlanAktifitasPTID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PalnAktifitasPtparentId).HasColumnName("PalnAktifitasPTParentID");

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasColumnName("period")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonBoid).HasColumnName("PersonBOID");

                entity.Property(e => e.PersonBoidpt).HasColumnName("PersonBOIDPT");

                entity.Property(e => e.PersonalTrainerId).HasColumnName("PersonalTrainerID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TrPlanAktifitasPt)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanAktifitasPT_tLocFitnessCenter");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TrPlanAktifitasPt)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanAktifitasPT_tMember");

                entity.HasOne(d => d.PalnAktifitasPtparent)
                    .WithMany(p => p.InversePalnAktifitasPtparent)
                    .HasForeignKey(d => d.PalnAktifitasPtparentId)
                    .HasConstraintName("FK_trPlanAktifitasPT_trPlanAktifitasPT");

                entity.HasOne(d => d.PersonBo)
                    .WithMany(p => p.TrPlanAktifitasPtPersonBo)
                    .HasForeignKey(d => d.PersonBoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanAktifitasPT_tUserBackOffice3");

                entity.HasOne(d => d.PersonBoidptNavigation)
                    .WithMany(p => p.TrPlanAktifitasPtPersonBoidptNavigation)
                    .HasForeignKey(d => d.PersonBoidpt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanAktifitasPT_tUserBackOffice2");

                entity.HasOne(d => d.PersonalTrainer)
                    .WithMany(p => p.TrPlanAktifitasPt)
                    .HasForeignKey(d => d.PersonalTrainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanAktifitasPT_trPersonalTrainer");
            });

            modelBuilder.Entity<TrPlanKelas>(entity =>
            {
                entity.HasKey(e => e.PlanKelasId);

                entity.ToTable("trPlanKelas");

                entity.Property(e => e.PlanKelasId).HasColumnName("PlanKelasID");

                entity.Property(e => e.KelasId).HasColumnName("KelasID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasColumnName("period")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonBoidadm).HasColumnName("PersonBOIDAdm");

                entity.Property(e => e.PersonBoidinstruktur).HasColumnName("PersonBOIDInstruktur");

                entity.HasOne(d => d.Kelas)
                    .WithMany(p => p.TrPlanKelas)
                    .HasForeignKey(d => d.KelasId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanKelas_tKelas");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TrPlanKelas)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanKelas_tLocFitnessCenter");

                entity.HasOne(d => d.PersonBoidadmNavigation)
                    .WithMany(p => p.TrPlanKelasPersonBoidadmNavigation)
                    .HasForeignKey(d => d.PersonBoidadm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanKelas_tUserBackOffice3");

                entity.HasOne(d => d.PersonBoidinstrukturNavigation)
                    .WithMany(p => p.TrPlanKelasPersonBoidinstrukturNavigation)
                    .HasForeignKey(d => d.PersonBoidinstruktur)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trPlanKelas_tUserBackOffice2");
            });

            modelBuilder.Entity<TrTtd>(entity =>
            {
                entity.ToTable("trTTD");

                entity.Property(e => e.TrTtdid).HasColumnName("TrTTDID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.TrMembershipId).HasColumnName("trMembershipID");

                entity.Property(e => e.TrPtid).HasColumnName("trPTID");

                entity.Property(e => e.TrTtd1)
                    .HasColumnName("trTTD")
                    .HasColumnType("image");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TrTtd)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trTTD_tMember1");

                entity.HasOne(d => d.MemberNavigation)
                    .WithMany(p => p.TrTtd)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_trTTD_tPerson");

                entity.HasOne(d => d.TrMembership)
                    .WithMany(p => p.TrTtd)
                    .HasForeignKey(d => d.TrMembershipId)
                    .HasConstraintName("FK_trTTD_trMembership");

                entity.HasOne(d => d.TrPt)
                    .WithMany(p => p.TrTtd)
                    .HasForeignKey(d => d.TrPtid)
                    .HasConstraintName("FK_trTTD_trPersonalTrainer");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
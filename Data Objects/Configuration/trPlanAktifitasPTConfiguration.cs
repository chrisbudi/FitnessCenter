// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

namespace DataObjects.Configuration
{
    using DataObjects.Context;
    using DataObjects.Entities;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.19.3.0")]
    public class trPlanAktifitasPTConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<trPlanAktifitasPT>
    {
        public trPlanAktifitasPTConfiguration()
            : this("dbo")
        {
        }

        public trPlanAktifitasPTConfiguration(string schema)
        {
            ToTable(schema + ".trPlanAktifitasPT");
            HasKey(x => x.PlanAktifitasPTID);

            Property(x => x.PlanAktifitasPTID).HasColumnName(@"PlanAktifitasPTID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PersonalTrainerID).HasColumnName(@"PersonalTrainerID").IsRequired().HasColumnType("int");
            Property(x => x.PalnAktifitasPTParentID).HasColumnName(@"PalnAktifitasPTParentID").IsOptional().HasColumnType("int");
            Property(x => x.DayOfWeek).HasColumnName(@"DayOfWeek").IsRequired().HasColumnType("int");
            Property(x => x.period).HasColumnName(@"period").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.WaktuMulai).HasColumnName(@"WaktuMulai").IsRequired().HasColumnType("time");
            Property(x => x.WaktuSelesai).HasColumnName(@"WaktuSelesai").IsRequired().HasColumnType("time");
            Property(x => x.LocationID).HasColumnName(@"LocationID").IsRequired().HasColumnType("int");
            Property(x => x.MemberID).HasColumnName(@"MemberID").IsRequired().HasColumnType("int");
            Property(x => x.PersonBOIDPT).HasColumnName(@"PersonBOIDPT").IsRequired().HasColumnType("int");
            Property(x => x.PersonBOID).HasColumnName(@"PersonBOID").IsRequired().HasColumnType("int");
            Property(x => x.Note).HasColumnName(@"Note").IsOptional().HasColumnType("nvarchar");

            HasOptional(a => a.trPlanAktifitasPT_PalnAktifitasPTParentID).WithMany(b => b.trPlanAktifitasPTs).HasForeignKey(c => c.PalnAktifitasPTParentID);
            HasRequired(a => a.tLocFitnessCenter).WithMany(b => b.trPlanAktifitasPTs).HasForeignKey(c => c.LocationID);
            HasRequired(a => a.tMember).WithMany(b => b.trPlanAktifitasPTs).HasForeignKey(c => c.MemberID);
            HasRequired(a => a.trPersonalTrainer).WithMany(b => b.trPlanAktifitasPTs).HasForeignKey(c => c.PersonalTrainerID);
            HasRequired(a => a.tUserBackOffice_PersonBOID).WithMany(b => b.trPlanAktifitasPTs_PersonBOID).HasForeignKey(c => c.PersonBOID);
            HasRequired(a => a.tUserBackOffice_PersonBOIDPT).WithMany(b => b.trPlanAktifitasPTs_PersonBOIDPT).HasForeignKey(c => c.PersonBOIDPT);
        }
    }

}
// </auto-generated>

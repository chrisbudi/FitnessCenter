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
    public class trMembershipConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<trMembership>
    {
        public trMembershipConfiguration()
            : this("dbo")
        {
        }

        public trMembershipConfiguration(string schema)
        {
            ToTable(schema + ".trMembership");
            HasKey(x => x.trMembershipID);

            Property(x => x.trMembershipID).HasColumnName(@"trMembershipID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ParentID).HasColumnName(@"ParentID").IsOptional().HasColumnType("int");
            Property(x => x.LocationID).HasColumnName(@"LocationID").IsRequired().HasColumnType("int");
            Property(x => x.MSTime).HasColumnName(@"MSTime").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);
            Property(x => x.MSTglMulai).HasColumnName(@"MSTglMulai").IsRequired().HasColumnType("date");
            Property(x => x.MSTglSelesai).HasColumnName(@"MSTglSelesai").IsRequired().HasColumnType("date");
            Property(x => x.PersonBOIDADM).HasColumnName(@"PersonBOIDADM").IsRequired().HasColumnType("int");
            Property(x => x.PersonBOIDSales).HasColumnName(@"PersonBOIDSales").IsRequired().HasColumnType("int");
            Property(x => x.StatusMID).HasColumnName(@"StatusMID").IsRequired().HasColumnType("int");
            Property(x => x.Subtotal).HasColumnName(@"Subtotal").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Disc).HasColumnName(@"Disc").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Total).HasColumnName(@"Total").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.GenBayar).HasColumnName(@"GenBayar").IsOptional().HasColumnType("int");
            Property(x => x.Note).HasColumnName(@"Note").IsOptional().HasColumnType("nvarchar");
            Property(x => x.MemberID).HasColumnName(@"MemberID").IsRequired().HasColumnType("int");
            Property(x => x.AgreementID).HasColumnName(@"AgreementID").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CountMember).HasColumnName(@"CountMember").IsRequired().HasColumnType("int");
            Property(x => x.seq).HasColumnName(@"seq").IsRequired().HasColumnType("int");
            Property(x => x.Admin).HasColumnName(@"Admin").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Prorate).HasColumnName(@"Prorate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.CardStatus).HasColumnName(@"CardStatus").IsRequired().HasColumnType("int");
            Property(x => x.TotalMonth).HasColumnName(@"TotalMonth").IsRequired().HasColumnType("int");
            Property(x => x.AccountingStatus).HasColumnName(@"AccountingStatus").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.ActivationCode).HasColumnName(@"ActivationCode").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);

            HasOptional(a => a.trMembership_ParentID).WithMany(b => b.trMemberships).HasForeignKey(c => c.ParentID);
            HasRequired(a => a.tCardStatu).WithMany(b => b.trMemberships).HasForeignKey(c => c.CardStatus);
            HasRequired(a => a.tLocFitnessCenter).WithMany(b => b.trMemberships).HasForeignKey(c => c.LocationID);
            HasRequired(a => a.tMember).WithMany(b => b.trMemberships).HasForeignKey(c => c.MemberID);
            HasRequired(a => a.tStatusMember).WithMany(b => b.trMemberships).HasForeignKey(c => c.StatusMID);
            HasRequired(a => a.tUserBackOffice_PersonBOIDADM).WithMany(b => b.trMemberships_PersonBOIDADM).HasForeignKey(c => c.PersonBOIDADM);
            HasRequired(a => a.tUserBackOffice_PersonBOIDSales).WithMany(b => b.trMemberships_PersonBOIDSales).HasForeignKey(c => c.PersonBOIDSales);
        }
    }

}
// </auto-generated>
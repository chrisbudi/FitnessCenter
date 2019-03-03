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
    public class strPaymentMemberConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<strPaymentMember>
    {
        public strPaymentMemberConfiguration()
            : this("dbo")
        {
        }

        public strPaymentMemberConfiguration(string schema)
        {
            ToTable(schema + ".strPaymentMember");
            HasKey(x => x.trPaymentID);

            Property(x => x.trPaymentID).HasColumnName(@"trPaymentID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.trMembershipID).HasColumnName(@"trMembershipID").IsOptional().HasColumnType("int");
            Property(x => x.pembayaranke).HasColumnName(@"pembayaranke").IsRequired().HasColumnType("int");
            Property(x => x.statusBayar).HasColumnName(@"statusBayar").IsRequired().HasColumnType("bit");
            Property(x => x.Tanggal).HasColumnName(@"Tanggal").IsRequired().HasColumnType("date");
            Property(x => x.TimeStamp).HasColumnName(@"TimeStamp").IsOptional().HasColumnType("timestamp").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);
            Property(x => x.MemberTypeID).HasColumnName(@"MemberTypeID").IsOptional().HasColumnType("int");
            Property(x => x.Note).HasColumnName(@"Note").IsOptional().HasColumnType("nvarchar");
            Property(x => x.MembershipDTLID).HasColumnName(@"MembershipDTLID").IsOptional().HasColumnType("int");

            HasOptional(a => a.tMemberType).WithMany(b => b.strPaymentMembers).HasForeignKey(c => c.MemberTypeID);
            HasOptional(a => a.trMembership_MembershipDTLID).WithMany(b => b.strPaymentMembers_MembershipDTLID).HasForeignKey(c => c.MembershipDTLID);
            HasOptional(a => a.trMembership_trMembershipID).WithMany(b => b.strPaymentMembers_trMembershipID).HasForeignKey(c => c.trMembershipID);
        }
    }

}
// </auto-generated>
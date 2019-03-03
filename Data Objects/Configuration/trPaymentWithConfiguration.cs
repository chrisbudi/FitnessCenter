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
    public class trPaymentWithConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<trPaymentWith>
    {
        public trPaymentWithConfiguration()
            : this("dbo")
        {
        }

        public trPaymentWithConfiguration(string schema)
        {
            ToTable(schema + ".trPaymentWith");
            HasKey(x => x.PaymentWithID);

            Property(x => x.PaymentWithID).HasColumnName(@"PaymentWithID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PaymentTypeID).HasColumnName(@"PaymentTypeID").IsRequired().HasColumnType("int");
            Property(x => x.BankID).HasColumnName(@"BankID").IsOptional().HasColumnType("int");
            Property(x => x.EDCID).HasColumnName(@"EDCID").IsOptional().HasColumnType("int");
            Property(x => x.ApprCode).HasColumnName(@"ApprCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.TraceCode).HasColumnName(@"TraceCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Pemegang).HasColumnName(@"Pemegang").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.NoKartu).HasColumnName(@"NoKartu").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ValidUntil).HasColumnName(@"ValidUntil").IsOptional().HasColumnType("date");
            Property(x => x.payAmount).HasColumnName(@"payAmount").IsRequired().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.I).HasColumnName(@"I").IsRequired().HasColumnType("bit");
            Property(x => x.paidAmount).HasColumnName(@"paidAmount").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.MBRAmount).HasColumnName(@"MBRAmount").IsOptional().HasColumnType("decimal").HasPrecision(18,2);

            HasOptional(a => a.tBank_BankID).WithMany(b => b.trPaymentWiths_BankID).HasForeignKey(c => c.BankID);
            HasOptional(a => a.tBank_EDCID).WithMany(b => b.trPaymentWiths_EDCID).HasForeignKey(c => c.EDCID);
            HasRequired(a => a.tPaymentType).WithMany(b => b.trPaymentWiths).HasForeignKey(c => c.PaymentTypeID);
        }
    }

}
// </auto-generated>

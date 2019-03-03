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
    public class tBankConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<tBank>
    {
        public tBankConfiguration()
            : this("dbo")
        {
        }

        public tBankConfiguration(string schema)
        {
            ToTable(schema + ".tBank");
            HasKey(x => x.BankID);

            Property(x => x.BankID).HasColumnName(@"BankID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.NamaBank).HasColumnName(@"NamaBank").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.EDC).HasColumnName(@"EDC").IsRequired().HasColumnType("bit");
        }
    }

}
// </auto-generated>
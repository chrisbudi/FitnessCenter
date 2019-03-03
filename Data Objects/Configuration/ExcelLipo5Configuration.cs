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
    public class ExcelLipo5Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ExcelLipo5>
    {
        public ExcelLipo5Configuration()
            : this("dbo")
        {
        }

        public ExcelLipo5Configuration(string schema)
        {
            ToTable(schema + ".ExcelLipo5");
            HasKey(x => new { x.No, x.Agreement });

            Property(x => x.No).HasColumnName(@"No").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Agreement).HasColumnName(@"Agreement").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.AgreementFix).HasColumnName(@"AgreementFix").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.MemberName).HasColumnName(@"MemberName").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(200);
            Property(x => x.MembershipType).HasColumnName(@"MembershipType").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(200);
            Property(x => x.MembershipPrFix).HasColumnName(@"MembershipPrFix").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.MembershipFix).HasColumnName(@"MembershipFix").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(200);
            Property(x => x.Monthly).HasColumnName(@"Monthly").IsOptional().HasColumnType("int");
            Property(x => x.Note).HasColumnName(@"Note").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(200);
        }
    }

}
// </auto-generated>

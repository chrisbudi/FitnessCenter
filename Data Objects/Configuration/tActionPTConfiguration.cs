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
    public class tActionPTConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<tActionPT>
    {
        public tActionPTConfiguration()
            : this("dbo")
        {
        }

        public tActionPTConfiguration(string schema)
        {
            ToTable(schema + ".tActionPT");
            HasKey(x => x.ActionPTID);

            Property(x => x.ActionPTID).HasColumnName(@"ActionPTID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ActionPTName).HasColumnName(@"ActionPTName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ActionPTKet).HasColumnName(@"ActionPTKet").IsOptional().HasColumnType("nvarchar");
        }
    }

}
// </auto-generated>
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
    public class StrActionKlaimConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<StrActionKlaim>
    {
        public StrActionKlaimConfiguration()
            : this("dbo")
        {
        }

        public StrActionKlaimConfiguration(string schema)
        {
            ToTable(schema + ".StrActionKlaim");
            HasKey(x => x.strActionClaimID);

            Property(x => x.strActionClaimID).HasColumnName(@"strActionClaimID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ClaimID).HasColumnName(@"ClaimID").IsRequired().HasColumnType("int");
            Property(x => x.ActionPTID).HasColumnName(@"ActionPTID").IsRequired().HasColumnType("int");

            HasRequired(a => a.tActionPT).WithMany(b => b.StrActionKlaims).HasForeignKey(c => c.ActionPTID);
        }
    }

}
// </auto-generated>
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
    public class strLocMemberConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<strLocMember>
    {
        public strLocMemberConfiguration()
            : this("dbo")
        {
        }

        public strLocMemberConfiguration(string schema)
        {
            ToTable(schema + ".strLocMember");
            HasKey(x => x.LocMemberID);

            Property(x => x.LocMemberID).HasColumnName(@"LocMemberID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.MemberID).HasColumnName(@"MemberID").IsRequired().HasColumnType("int");
            Property(x => x.LocationID).HasColumnName(@"LocationID").IsRequired().HasColumnType("int");

            HasRequired(a => a.tLocFitnessCenter).WithMany(b => b.strLocMembers).HasForeignKey(c => c.LocationID);
            HasRequired(a => a.tMember).WithMany(b => b.strLocMembers).HasForeignKey(c => c.MemberID);
        }
    }

}
// </auto-generated>

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
    public class tPosisiConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<tPosisi>
    {
        public tPosisiConfiguration()
            : this("dbo")
        {
        }

        public tPosisiConfiguration(string schema)
        {
            ToTable(schema + ".tPosisi");
            HasKey(x => x.PosisiID);

            Property(x => x.PosisiID).HasColumnName(@"PosisiID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PNamaPosisi).HasColumnName(@"PNamaPosisi").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
        }
    }

}
// </auto-generated>

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

namespace DataObjects.Entities
{
	using System.ComponentModel.DataAnnotations;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.19.3.0")]
	[MetadataType(typeof(DataValidator.Validation.ValidationstrLocMember))]
    public partial class strLocMember
    {
        public int LocMemberID { get; set; }
        public int MemberID { get; set; }
        public int LocationID { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }
        public virtual tMember tMember { get; set; }

        public strLocMember()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

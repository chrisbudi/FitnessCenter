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
    public partial class stMemberFingerPrint
    {
        public int stMemberFingerprintId { get; set; }
        public int? MemberId { get; set; }
        public string note { get; set; }
        public byte[] pict1 { get; set; }
        public byte[] pict2 { get; set; }

        public virtual tMember tMember { get; set; }

        public stMemberFingerPrint()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

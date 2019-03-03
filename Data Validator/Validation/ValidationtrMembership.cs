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

namespace DataValidator.Validation
{
    using System.ComponentModel.DataAnnotations;

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.19.3.0")]
    public class ValidationtrMembership
    {
        public int trMembershipID { get; set; }
        public int? ParentID { get; set; }
        public int LocationID { get; set; }
        public byte[] MSTime { get; set; }
        public System.DateTime MSTglMulai { get; set; }
        public System.DateTime MSTglSelesai { get; set; }
        public int PersonBOIDADM { get; set; }
        [Required(ErrorMessage = "Data sales di perlukan")]
        public int PersonBOIDSales { get; set; }
        public int StatusMID { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Disc { get; set; }
        public decimal? Total { get; set; }
        public int? GenBayar { get; set; }
        public string Note { get; set; }
        public int MemberID { get; set; }
        public string AgreementID { get; set; }
        public int CountMember { get; set; }
        public int seq { get; set; }
        public decimal? Admin { get; set; }
        public decimal? Prorate { get; set; }
        public int CardStatus { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int TotalMonth { get; set; }

        public string AccountingStatus { get; set; }
        public string ActivationCode { get; set; }


    }

}
// </auto-generated>

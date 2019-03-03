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
	[MetadataType(typeof(DataValidator.Validation.ValidationtrPembayaran))]
    public partial class trPembayaran
    {
        public int IDPEMBAYARAN { get; set; }
        public byte[] TS { get; set; }
        public int? MemberID { get; set; }
        public int ID_Transaksi { get; set; }
        public System.DateTime D_TGL { get; set; }
        public decimal? D_JUMLAH { get; set; }
        public int PersonBOIDFin { get; set; }
        public decimal? CC { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Cash { get; set; }
        public string TransaksiTipe { get; set; }

        public virtual tMember tMember { get; set; }
        public virtual tUserBackOffice tUserBackOffice { get; set; }

        public trPembayaran()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

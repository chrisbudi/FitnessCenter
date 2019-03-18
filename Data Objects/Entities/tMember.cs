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
	[MetadataType(typeof(DataValidator.Validation.ValidationtMember))]
    public partial class tMember
    {
        public int MemberID { get; set; }
        public string MemberNO { get; set; }
        public int PersonID { get; set; }
        public string MRFID { get; set; }
        public string MFotoSignature { get; set; }
        public string MFoto { get; set; }
        public string MFotoKTP { get; set; }
        public string MFotoCCDebit { get; set; }
        public int? MemberTypeID { get; set; }
        public string MFotoUrl { get; set; }

        public virtual System.Collections.Generic.ICollection<stMemberFingerPrint> stMemberFingerPrints { get; set; }
        public virtual System.Collections.Generic.ICollection<strLocMember> strLocMembers { get; set; }
        public virtual System.Collections.Generic.ICollection<trAktifitasMember> trAktifitasMembers { get; set; }
        public virtual System.Collections.Generic.ICollection<trMembership> trMemberships { get; set; }
        public virtual System.Collections.Generic.ICollection<trPembayaran> trPembayarans { get; set; }
        public virtual System.Collections.Generic.ICollection<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }
        public virtual System.Collections.Generic.ICollection<trTTD> trTTDs { get; set; }

        public virtual tMemberType tMemberType { get; set; }
        public virtual tPerson tPerson { get; set; }

        public tMember()
        {
            stMemberFingerPrints = new System.Collections.Generic.List<stMemberFingerPrint>();
            strLocMembers = new System.Collections.Generic.List<strLocMember>();
            trAktifitasMembers = new System.Collections.Generic.List<trAktifitasMember>();
            trMemberships = new System.Collections.Generic.List<trMembership>();
            trPembayarans = new System.Collections.Generic.List<trPembayaran>();
            trPlanAktifitasPTs = new System.Collections.Generic.List<trPlanAktifitasPT>();
            trTTDs = new System.Collections.Generic.List<trTTD>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

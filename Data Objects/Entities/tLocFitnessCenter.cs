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
	[MetadataType(typeof(DataValidator.Validation.ValidationtLocFitnessCenter))]
    public partial class tLocFitnessCenter
    {
        public int LocationID { get; set; }
        public string LAlamat { get; set; }
        public string LTlp { get; set; }
        public string LFax { get; set; }
        public string LAuth { get; set; }
        public System.Data.Entity.Spatial.DbGeography LSpatial { get; set; }

        public virtual System.Collections.Generic.ICollection<strLocBO> strLocBOes { get; set; }
        public virtual System.Collections.Generic.ICollection<strLocMember> strLocMembers { get; set; }
        public virtual System.Collections.Generic.ICollection<tMemberType> tMemberTypes { get; set; }
        public virtual System.Collections.Generic.ICollection<trAktifitasMember> trAktifitasMembers { get; set; }
        public virtual System.Collections.Generic.ICollection<trMembership> trMemberships { get; set; }
        public virtual System.Collections.Generic.ICollection<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }
        public virtual System.Collections.Generic.ICollection<trPlanKela> trPlanKelas { get; set; }

        public tLocFitnessCenter()
        {
            strLocBOes = new System.Collections.Generic.List<strLocBO>();
            strLocMembers = new System.Collections.Generic.List<strLocMember>();
            tMemberTypes = new System.Collections.Generic.List<tMemberType>();
            trAktifitasMembers = new System.Collections.Generic.List<trAktifitasMember>();
            trMemberships = new System.Collections.Generic.List<trMembership>();
            trPlanAktifitasPTs = new System.Collections.Generic.List<trPlanAktifitasPT>();
            trPlanKelas = new System.Collections.Generic.List<trPlanKela>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
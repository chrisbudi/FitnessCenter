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
	[MetadataType(typeof(DataValidator.Validation.ValidationtrPlanKela))]
    public partial class trPlanKela
    {
        public int PlanKelasID { get; set; }
        public int KelasID { get; set; }
        public int PersonBOIDInstruktur { get; set; }
        public int PersonBOIDAdm { get; set; }
        public int DayOfWeek { get; set; }
        public string period { get; set; }
        public System.TimeSpan WaktuMulai { get; set; }
        public System.TimeSpan WaktuSelesai { get; set; }
        public int LocationID { get; set; }

        public virtual System.Collections.Generic.ICollection<trAktifitasKela> trAktifitasKelas { get; set; }

        public virtual tKela tKela { get; set; }
        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }
        public virtual tUserBackOffice tUserBackOffice_PersonBOIDAdm { get; set; }
        public virtual tUserBackOffice tUserBackOffice_PersonBOIDInstruktur { get; set; }

        public trPlanKela()
        {
            trAktifitasKelas = new System.Collections.Generic.List<trAktifitasKela>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
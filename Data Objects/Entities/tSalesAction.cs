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
	[MetadataType(typeof(DataValidator.Validation.ValidationtSalesAction))]
    public partial class tSalesAction
    {
        public int SalesActionID { get; set; }
        public string ActionName { get; set; }
        public string Note { get; set; }

        public virtual System.Collections.Generic.ICollection<strAktivitasSale> strAktivitasSales { get; set; }

        public tSalesAction()
        {
            strAktivitasSales = new System.Collections.Generic.List<strAktivitasSale>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
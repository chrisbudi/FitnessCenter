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
	[MetadataType(typeof(DataValidator.Validation.ValidationtEventScore))]
    public partial class tEventScore
    {
        public int EvScoreId { get; set; }
        public string EvScoreName { get; set; }
        public int? EvStepId { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set; }

        public virtual System.Collections.Generic.ICollection<trEventScore> trEventScores { get; set; }

        public virtual tEventStep tEventStep { get; set; }

        public tEventScore()
        {
            trEventScores = new System.Collections.Generic.List<trEventScore>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
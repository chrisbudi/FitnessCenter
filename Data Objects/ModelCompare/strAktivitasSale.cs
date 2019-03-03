namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class strAktivitasSale
    {
        [Key]
        public int AktivitasSalesID { get; set; }

        public int trMembersipID { get; set; }

        public int SalesActionID { get; set; }

        public int MemberStateID { get; set; }

        public string Note { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] aktivitasstap { get; set; }

        public virtual tMemberState tMemberState { get; set; }

        public virtual trMembership trMembership { get; set; }

        public virtual tSalesAction tSalesAction { get; set; }
    }
}

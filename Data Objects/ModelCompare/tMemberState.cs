namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tMemberState")]
    public partial class tMemberState
    {
        public tMemberState()
        {
            strAktivitasSales = new HashSet<strAktivitasSale>();
        }

        [Key]
        public int MemberStateID { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberStateName { get; set; }

        public string Note { get; set; }

        public virtual ICollection<strAktivitasSale> strAktivitasSales { get; set; }
    }
}

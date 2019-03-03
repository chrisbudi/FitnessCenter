namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trTTD")]
    public partial class trTTD
    {
        public int TrTTDID { get; set; }

        public int? trMembersipID { get; set; }

        public int? trPTID { get; set; }

        [Column("trTTD", TypeName = "image")]
        public byte[] trTTD1 { get; set; }

        public int PersonID { get; set; }

        public virtual tPerson tPerson { get; set; }

        public virtual trMembership trMembership { get; set; }

        public virtual trPersonalTrainer trPersonalTrainer { get; set; }
    }
}

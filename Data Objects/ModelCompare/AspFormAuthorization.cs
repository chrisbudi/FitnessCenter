namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspFormAuthorization")]
    public partial class AspFormAuthorization
    {
        [Key]
        public int authID { get; set; }

        public int FormID { get; set; }

        [Required]
        [StringLength(128)]
        public string GroupId { get; set; }

        public virtual ApplicationGroup ApplicationGroup { get; set; }

        public virtual AspForm AspForm { get; set; }
    }
}

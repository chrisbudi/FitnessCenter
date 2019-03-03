namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspForm")]
    public partial class AspForm
    {
        public AspForm()
        {
            AspForm1 = new HashSet<AspForm>();
            AspFormAuthorizations = new HashSet<AspFormAuthorization>();
        }

        [Key]
        public int FormId { get; set; }

        public int? parent_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Module { get; set; }

        [Required]
        [StringLength(50)]
        public string title { get; set; }

        [StringLength(255)]
        public string desciption { get; set; }

        [StringLength(50)]
        public string controller { get; set; }

        [StringLength(50)]
        public string action { get; set; }

        [StringLength(255)]
        public string url { get; set; }

        [StringLength(50)]
        public string area { get; set; }

        public bool? visible { get; set; }

        [StringLength(50)]
        public string parameter { get; set; }

        public bool? clickable { get; set; }

        [StringLength(10)]
        public string statusCode { get; set; }

        public int? displayOrder { get; set; }

        [StringLength(50)]
        public string iconclass { get; set; }

        public virtual ICollection<AspForm> AspForm1 { get; set; }

        public virtual AspForm AspForm2 { get; set; }

        public virtual ICollection<AspFormAuthorization> AspFormAuthorizations { get; set; }
    }
}

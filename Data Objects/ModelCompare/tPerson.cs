namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tPerson")]
    public partial class tPerson
    {
        public tPerson()
        {
            tMembers = new HashSet<tMember>();
            trMemberships = new HashSet<trMembership>();
            trTTDs = new HashSet<trTTD>();
            tUserBackOffices = new HashSet<tUserBackOffice>();
        }

        [Key]
        public int PersonID { get; set; }

        [Required]
        [StringLength(50)]
        public string PNama { get; set; }

        [StringLength(50)]
        public string PTempLahir { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PTglLahir { get; set; }

        [StringLength(1)]
        public string PGender { get; set; }

        [StringLength(200)]
        public string PAlamat { get; set; }

        [StringLength(3)]
        public string PRT { get; set; }

        [StringLength(3)]
        public string PRW { get; set; }

        [StringLength(50)]
        public string PKelurahan { get; set; }

        [StringLength(50)]
        public string PKecamatan { get; set; }

        [StringLength(50)]
        public string PKota { get; set; }

        [StringLength(50)]
        public string PPropinsi { get; set; }

        [StringLength(50)]
        public string PIdentitas { get; set; }

        [StringLength(50)]
        public string PEmail { get; set; }

        [StringLength(50)]
        public string PTelp { get; set; }

        [StringLength(50)]
        public string PHP1 { get; set; }

        [StringLength(15)]
        public string PHP2 { get; set; }

        [StringLength(10)]
        public string PPinBB { get; set; }

        [StringLength(128)]
        public string Id { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ICollection<tMember> tMembers { get; set; }

        public virtual ICollection<trMembership> trMemberships { get; set; }

        public virtual ICollection<trTTD> trTTDs { get; set; }

        public virtual ICollection<tUserBackOffice> tUserBackOffices { get; set; }
    }
}

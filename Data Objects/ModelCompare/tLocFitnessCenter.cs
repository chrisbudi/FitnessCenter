namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tLocFitnessCenter")]
    public partial class tLocFitnessCenter
    {
        public tLocFitnessCenter()
        {
            strLocBOes = new HashSet<strLocBO>();
            strLocMembers = new HashSet<strLocMember>();
            tMemberTypes = new HashSet<tMemberType>();
            trAktifitasMembers = new HashSet<trAktifitasMember>();
            trMemberships = new HashSet<trMembership>();
            trPersonalTrainers = new HashSet<trPersonalTrainer>();
            trPlanAktifitasPTs = new HashSet<trPlanAktifitasPT>();
            trPlanKelas = new HashSet<trPlanKela>();
        }

        [Key]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string LAlamat { get; set; }

        [Required]
        [StringLength(50)]
        public string LTlp { get; set; }

        [Required]
        [StringLength(50)]
        public string LFax { get; set; }

        [Required]
        [StringLength(50)]
        public string LAuth { get; set; }

        public DbGeography LSpatial { get; set; }

        public virtual ICollection<strLocBO> strLocBOes { get; set; }

        public virtual ICollection<strLocMember> strLocMembers { get; set; }

        public virtual ICollection<tMemberType> tMemberTypes { get; set; }

        public virtual ICollection<trAktifitasMember> trAktifitasMembers { get; set; }

        public virtual ICollection<trMembership> trMemberships { get; set; }

        public virtual ICollection<trPersonalTrainer> trPersonalTrainers { get; set; }

        public virtual ICollection<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }

        public virtual ICollection<trPlanKela> trPlanKelas { get; set; }
    }
}

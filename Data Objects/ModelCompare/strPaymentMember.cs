namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("strPaymentMember")]
    public partial class strPaymentMember
    {
        public strPaymentMember()
        {
            strPayments = new HashSet<strPayment>();
        }

        [Key]
        public int trPaymentID { get; set; }

        public int trMembersipID { get; set; }

        public int pembayaranke { get; set; }

        public bool statusBayar { get; set; }

        [Column(TypeName = "date")]
        public DateTime Tanggal { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] TimeStamp { get; set; }

        public int? MemberTypeID { get; set; }

        public string Note { get; set; }

        public virtual ICollection<strPayment> strPayments { get; set; }

        public virtual tMemberType tMemberType { get; set; }

        public virtual trMembership trMembership { get; set; }
    }
}

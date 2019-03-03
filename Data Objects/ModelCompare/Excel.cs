namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Excel")]
    public partial class Excel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int No { get; set; }

        [StringLength(50)]
        public string AggreementNo { get; set; }

        [StringLength(50)]
        public string FixAgrrementNo { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Type { get; set; }

        [StringLength(150)]
        public string FixType { get; set; }

        public decimal? PaymentAdmin { get; set; }

        public decimal? Payment { get; set; }

        public decimal? Payment2 { get; set; }

        public DateTime? JoinDate { get; set; }

        [StringLength(150)]
        public string PaymentBy { get; set; }

        [StringLength(50)]
        public string PaymentDesc { get; set; }

        [StringLength(200)]
        public string PaymentNotes { get; set; }

        [StringLength(50)]
        public string Telp { get; set; }

        public DateTime? Birth { get; set; }

        [StringLength(40)]
        public string SalesName { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public int? Bag { get; set; }

        public int? Tshirt { get; set; }

        [StringLength(150)]
        public string Membersince { get; set; }

        public DateTime? ExpDate { get; set; }
    }
}

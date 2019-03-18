using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class ExcelUploadDb
    {
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Agreement { get; set; }
        public string AgreementFix { get; set; }
        public string MemberName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Telp { get; set; }
        public string MembershipType { get; set; }
        public string MembershipTypeFix { get; set; }
        public string RecordType { get; set; }
        public int? LongTime { get; set; }
        public DateTime? Efd { get; set; }
        public DateTime? Exp { get; set; }
        public decimal? Paid1 { get; set; }
        public decimal? Paid2 { get; set; }
        public decimal? Paid3 { get; set; }
        public decimal? Mbr1 { get; set; }
        public decimal? Mbr2 { get; set; }
        public string By1 { get; set; }
        public string By2 { get; set; }
        public string Sold { get; set; }
        public string Remarks { get; set; }
        public string Address { get; set; }
        public bool? GymBag { get; set; }
        public bool? Tshirt { get; set; }
        public string Bank { get; set; }
        public string Payment { get; set; }
        public bool? Updated { get; set; }
        public string Prfix { get; set; }
    }
}
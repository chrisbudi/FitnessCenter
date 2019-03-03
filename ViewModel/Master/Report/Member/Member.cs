using System;

namespace ViewModel.Master.Report.Member
{
    public class Member
    {
        public string Nama { get; set; }
        public string identitas { get; set; }
        public DateTime tglLahir { get; set; }
        public string JenisKelamin { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string HP { get; set; }
        public string HP2 { get; set; }
        public string Telp { get; set; }
        public string TypeMember { get; set; }
        public decimal fistMonthDues { get; set; }
        public decimal lastMonthDues { get; set; }
        public decimal AdministrationFee { get; set; }
        public decimal fullPayment { get; set; }
        public decimal Total { get; set; }
        public decimal GetMembershipFor { get; set; }
        public DateTime KeanggotaanMulai { get; set; }
        public string KeanggotaanNo { get; set; }
        public string Barcode { get; set; }
    }
}
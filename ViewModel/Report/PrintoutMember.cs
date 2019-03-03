using System;

namespace ViewModel.Report
{
    public class PrintoutMember
    {
        #region PersonalData
        public string Cabang { get; set; }
        public string Nama { get; set; }
        public string Identitas { get; set; }
        public DateTime TglLahir { get; set; }
        public string JenisKelamin { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string HP1 { get; set; }
        public string TelpRumah { get; set; }
        #endregion

        #region Payment
        public string TypeMembership { get; set; }
        public decimal FirstDues { get; set; }
        public decimal LastDues { get; set; }
        public decimal? AdministrationFee { get; set; }
        public decimal FullPayment { get; set; }
        public decimal Total { get; set; }
        public int GetMembershipFor { get; set; }
        #endregion

        #region Keanggotaan
        public DateTime KeanggotaanMulai { get; set; }
        public string No { get; set; }
        public string Barcode { get; set; }
        #endregion

        #region Credit
        public string JenisKartu { get; set; }
        public string Bank { get; set; }
        public DateTime ValidUntil { get; set; }
        public string NamaPemegang { get; set; }
        public string NoKartu { get; set; }
        public string IuranBulanans { get; set; }
        public string LokerSepatu { get; set; }
        #endregion
    }
}

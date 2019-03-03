using CrystalDecisions.Shared;
using System;

namespace ViewModel.Report
{
    public class LaporanSalesDetail
    {
        public string TransactionType { get; set; }
        public string TransactionDetailType { get; set; }
        public int countNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string memberid { get; set; }
        public string nama { get; set; }
        public DateTime DOB { get; set; }
        public string Phone { get; set; }
        public DateTime expired { get; set; }
        public decimal admPrice { get; set; }
        public decimal Prorate { get; set; }
        public decimal Amount { get; set; }
        public string payType { get; set; }
        public string Bank { get; set; }
        public string Remark { get; set; }
        public string InputAdmin { get; set; }
        public string Sales { get; set; }
        public int Month { get; set; }
    }

    public class LaporanSalesSummary
    {
        public string TransaksiType { get; set; }
        public string JenisTransaksi { get; set; }
        public int countNo { get; set; }
        public int TotalMember { get; set; }
        public decimal totalAdmin { get; set; }
        public decimal totalPaid { get; set; }
    }
}
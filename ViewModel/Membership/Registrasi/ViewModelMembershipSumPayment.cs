using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Membership.Registrasi
{
    public class ViewModelMasterPayment
    {
        public DateTime? MSTglMulai { get; set; }

        public Decimal? Total { get; set; }

        public Decimal? Cash { get; set; }

        public Decimal? Debit { get; set; }

        public Decimal? Credit { get; set; }

        public Decimal? TotalPayment { get; set; }

        [Range(0, 0, ErrorMessage = "Value must equals 0")]
        [Required]
        public Decimal? RemainPayment { get; set; }

        [Range(0, 0, ErrorMessage = "Value must equals 0")]
        [Required]
        public Decimal? TotalOverPayment { get; set; }

        public int? MemberType { get; set; }
        public int CountMember { get; set; }
        public int TotalMonth { get; set; }

    }
}
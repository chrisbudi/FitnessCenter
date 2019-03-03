using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Membership.Registrasi
{
    public class ViewModelMembershipPayment
    {
        public int seq { get; set; }

        public string PNama { get; set; }

        public int? TotalMonth { get; set; }

        public decimal? Totalpayment { get; set; }

        //        [Required]
        public string Note { get; set; }

        [Required]
        public DateTime startDatetime { get; set; }

    }
}

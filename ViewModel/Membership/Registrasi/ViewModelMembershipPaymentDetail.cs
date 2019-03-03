using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViewModel.Membership.Registrasi
{
    public class ViewModelMembershipPaymentDetail
    {
        [Key]
        public int PaymentWithID { get; set; }

        [Display(Name = "Seq")]
        public int Seq { get; set; }

        [Display(Name = "Bank")]
        public int Bank { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "Trace Code")]
        public string TraceCode { get; set; }


        [StringLength(50)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "Approval Code")]
        public string ApprCode { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }

        public bool Instalment { get; set; }

        public string PaymentType { get; set; }

        public string AgreementId { get; set; }

        public decimal PaymentAmount { get; set; }
    }
}

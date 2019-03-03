using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViewModel.Trainer.RegisterPT
{
    public class ViewModelPaymentDetail
    {
        [Key]
        public int PaymentWithID { get; set; }

        [Display(Name = "Seq")]
        public int Seq { get; set; }

        [Display(Name = "Bank")]
        public int Bank { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "Card Holder")]
        public string Pemegang { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "Card No")]
        public string NoKartu { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }

        public string PaymentType { get; set; }

        public decimal PaymentAmount { get; set; }
    }
}

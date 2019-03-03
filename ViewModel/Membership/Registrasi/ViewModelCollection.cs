using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.Entities;

namespace ViewModel.Membership.Registrasi
{
    public class ViewModelCollection
    {
        public strPaymentMember PaymentMember { get; set; }
        public trPaymentWith PaymentWith { get; set; }
        public int PaymentNo { get; set; }

        public DateTime PaymentDateFor { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.Entities;

namespace ViewModel.Master
{
    public class StatusMemberPriceViewModel
    {
        public tStatusMember StatusMember { get; set; }
        public decimal Price { get; set; }
        public string Period { get; set; }
        public int StatusPriceId { get; set; }
    }
}

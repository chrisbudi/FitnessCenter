using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TStatusMemberPrice
    {
        public int TStatusMemberPriceId { get; set; }
        public int StatusMid { get; set; }
        public decimal Price { get; set; }
        public string Period { get; set; }

        public virtual TStatusMember StatusM { get; set; }
    }
}
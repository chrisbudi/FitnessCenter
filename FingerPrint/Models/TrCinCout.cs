using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrCinCout
    {
        public int CinCoutId { get; set; }
        public byte[] RefCinCout { get; set; }
        public int PersonBoid { get; set; }
        public DateTime TimeStatus { get; set; }
        public int TypeStatusInOut { get; set; }
        public int LocBoId { get; set; }

        public virtual StrLocBo LocBo { get; set; }
        public virtual TUserBackOffice PersonBo { get; set; }
        public virtual TTypeStatusCinCout TypeStatusInOutNavigation { get; set; }
    }
}
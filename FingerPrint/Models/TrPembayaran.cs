using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrPembayaran
    {
        public int Idpembayaran { get; set; }
        public byte[] Ts { get; set; }
        public int? MemberId { get; set; }
        public int IdTransaksi { get; set; }
        public DateTime DTgl { get; set; }
        public decimal? DJumlah { get; set; }
        public int PersonBoidfin { get; set; }
        public decimal? Cc { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Cash { get; set; }
        public string TransaksiTipe { get; set; }

        public virtual TMember Member { get; set; }
        public virtual TUserBackOffice PersonBoidfinNavigation { get; set; }
    }
}
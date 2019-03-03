using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Membership.FreezeCancel
{
    public class PrintoutCancel
    {
        public string Nama { get; set; }
        public string identitas { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string HP { get; set; }
        public string HP2 { get; set; }
        public string Telp { get; set; }
        public string Barcode { get; set; }
        public DateTime tglHariIni { get; set; }
        public decimal CFP { get; set; }
        public string lokasiKlub { get; set; }
        public string namaStaf { get; set; }
        public string BOID { get; set; }
        public DateTime tglBatal { get; set; }
        public int statusNo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Trainer.RegisterPT
{
    public class ViewModelPrintRegisterPersonalTrainer
    {
        public string NOAnggota { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Handphone { get; set; }
        public string NoIdentitas { get; set; }
        public string Telp { get; set; }
        public DateTime TanggalCetak { get; set; }
        public string NamaTrainer { get; set; }
        public int JumlahPaket { get; set; }
        public int Lvl { get; set; }
        public DateTime TanggalExpired { get; set; }
        public decimal HargaPerSesi { get; set; }
        public decimal JumlahKontrak { get; set; }


    }
}

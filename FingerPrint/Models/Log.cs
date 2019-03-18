using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Identity { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public DateTime? Utcdate { get; set; }
        public string IdTable { get; set; }
        public string NamaTransaksi { get; set; }
        public string NamaTable { get; set; }
        public string Exception { get; set; }
    }
}
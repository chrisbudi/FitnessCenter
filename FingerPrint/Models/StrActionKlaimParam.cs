using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrActionKlaimParam
    {
        public int IdPar { get; set; }
        public int StrActionClaimId { get; set; }
        public string NamaParam { get; set; }
        public string Satuan { get; set; }
        public string Value { get; set; }

        public virtual StrActionKlaim StrActionClaim { get; set; }
    }
}
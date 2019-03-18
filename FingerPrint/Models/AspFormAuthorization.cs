using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class AspFormAuthorization
    {
        public int AuthId { get; set; }
        public int FormId { get; set; }
        public string GroupId { get; set; }

        public virtual AspForm Form { get; set; }
        public virtual ApplicationGroups Group { get; set; }
    }
}
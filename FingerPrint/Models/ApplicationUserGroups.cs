using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class ApplicationUserGroups
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationGroupId { get; set; }

        public virtual ApplicationGroups ApplicationGroup { get; set; }
    }
}
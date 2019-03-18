using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class AspNetUserRoles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class AspForm
    {
        public AspForm()
        {
            AspFormAuthorization = new HashSet<AspFormAuthorization>();
            InverseParentNavigation = new HashSet<AspForm>();
        }

        public int FormId { get; set; }
        public int? ParentId { get; set; }
        public string MasterModule { get; set; }
        public string Module { get; set; }
        public string Title { get; set; }
        public string Desciption { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Area { get; set; }
        public bool? Visible { get; set; }
        public string Parameter { get; set; }
        public bool? Clickable { get; set; }
        public string StatusCode { get; set; }
        public int? DisplayOrder { get; set; }
        public string Iconclass { get; set; }
        public bool? Parent { get; set; }

        public virtual AspForm ParentNavigation { get; set; }
        public virtual ICollection<AspFormAuthorization> AspFormAuthorization { get; set; }
        public virtual ICollection<AspForm> InverseParentNavigation { get; set; }
    }
}
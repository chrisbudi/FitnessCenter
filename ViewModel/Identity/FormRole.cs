using System.Collections.Generic;
using IdentityModel.Model;
using Services.Class;

namespace ViewModel.Identity
{
    public class FormRole
    {
        public AspFormAuthorization AspFormAuthorization { get; set; }
        public IEnumerable<AspForm> AspForms { get; set; }
        public List<Select2StringResult> ApplicationGroups { get; set; }
    }
}

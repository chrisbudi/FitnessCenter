using System.Collections.Generic;
using IdentityModel.Model;

namespace IdentityModel.Principal
{
    public class IdentityPrincipalSerializeModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public PersonIdentity.Person Person { get; set; }
        public PersonIdentity.Member Member { get; set; }
        public PersonIdentity.BackOffice BackOffice { get; set; }
        public string LocationsId { get; set; }
        public int ActiveLocation { get; set; }
    }
}

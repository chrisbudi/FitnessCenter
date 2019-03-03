using System.Collections.Generic;
using System.Security.Principal;
using IdentityModel.Model;

namespace IdentityModel.Principal
{
    public class IdentityPrincipal : IIdentityPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public IdentityPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public PersonIdentity.Person Person { get; set; }
        public PersonIdentity.Member Member { get; set; }
        public PersonIdentity.BackOffice BackOffice { get; set; }
        public string LocationsId { get; set; }
        public int ActiveLocation { get; set; }
    }
}

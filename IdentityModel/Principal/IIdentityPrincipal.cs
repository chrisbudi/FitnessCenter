using System.Collections.Generic;
using System.Security.Principal;
using IdentityModel.Model;

namespace IdentityModel.Principal
{
    interface IIdentityPrincipal : IPrincipal
    {
        string Id { get; set; }
        string UserName { get; set; }
        string LocationsId { get; set; }
        PersonIdentity.Person Person { get; set; }
        PersonIdentity.Member Member { get; set; }
        PersonIdentity.BackOffice BackOffice { get; set; }
        int ActiveLocation { get; set; }

    }
}

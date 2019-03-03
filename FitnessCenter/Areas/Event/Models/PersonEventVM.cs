using System.Collections.Generic;
using DataObjects.Entities;

namespace FitnessCenter.Areas.Event.Models
{
    public class PersonEventVm
    {
        public trPersonEvent PersonEvent { get; set; }

        public ICollection<tRoleEvent> RoleEvents { get; set; }

    }
}

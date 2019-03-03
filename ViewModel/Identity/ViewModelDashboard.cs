using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.Entities;

namespace ViewModel.Identity
{
    public class ViewModelDashboard
    {
        public IEnumerable<trMembership> Membership { get; set; }

        public IEnumerable<trPersonalTrainer> PersonalTrainers { get; set; }
         
        public IEnumerable<trAktifitasMember> MemberActivity { get; set; }  
    }
}

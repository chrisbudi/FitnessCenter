using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.Entities;

namespace ViewModel.Accounting
{
    public class ViewModelDailyClosing
    {
        public IEnumerable<trMembership> trMemberships { get; set; }
        public IEnumerable<trPersonalTrainer> trPersonalTrainers { get; set; }
    }
}

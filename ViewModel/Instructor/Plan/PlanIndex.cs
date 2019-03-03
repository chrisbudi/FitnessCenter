using System.Collections.Generic;
using DataObjects.Entities;

namespace ViewModel.Instructor.Plan
{
    public class InstructorPlanIndex
    {
        public IEnumerable<tKela> Kelases { get; set; }
    
        public tKela Kelas { get; set; }
    }
}

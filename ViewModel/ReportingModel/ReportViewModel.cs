using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ReportingModel
{
    public class ReportViewModel
    {
        public string ReportType { get; set; }
        public DateTime? ReportBegin { get; set; }
        public DateTime? ReportEnd { get; set; }

    }
}

using System.Diagnostics.CodeAnalysis;
using DataObjects.Entities;

namespace ViewModel.Trainer.PlanPT
{
    public class ViewModelEventCalendar
    {
        public ViewModelEventCalendar()
        {
            PlanPT = new trPlanAktifitasPT();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string Url { get; set; }

        public string BackgroundColor { get; set; }

        public bool AllDay { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string BackOffice { get; set; }

        public string Name { get; set; }

        public trPlanAktifitasPT PlanPT { get; set; }

        public trPlanKela PlanInstructor { get; set; }
    }
}

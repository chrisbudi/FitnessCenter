using System.Web.Mvc;
using FitnessCenter.Models.Crystal_Report;

namespace FitnessCenter.Controllers
{
    public class ReportingController : FitController
    {
        public ActionResult ReportPage(string link)
        {
            return PartialView("Report/ReportPage", new ReportModel() { Url = link, Title = ""});
        }
    }
}
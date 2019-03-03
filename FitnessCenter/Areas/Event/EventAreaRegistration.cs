using System.Web.Mvc;

namespace FitnessCenter.Areas.Event
{
    public class EventAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Event";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Event_default",
                "Event/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
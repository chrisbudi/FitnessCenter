﻿using System.Web.Mvc;

namespace FitnessCenter.Areas.Tools
{
    public class GeneralAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tools";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tools_default",
                "Tools/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
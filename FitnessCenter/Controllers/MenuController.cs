using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessService.Identity;
using FitnessCenter.Models;
using Services.Extensions;

namespace FitnessCenter.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu

        // GET: Menu
        [HttpGet]
        public ActionResult TopBar(string menu, string controller)
        {
            if (User.Identity.IsAuthenticated)
            {
                var da = new FormsDataService();

                var activeMenu = da.GetActiveMenuByController(menu);

                var top = da.LoadModuleForms(User.Identity.Name, activeMenu);

                return PartialView("_TopBar", top);
            }
            return PartialView("_NullView");
        }

        [HttpGet]
        public ActionResult SideBar(string controllerModel, string actionModel)
        {
//            if (Menu.Nav != null)
//            {
//                return PartialView("_SideBar", Menu.Nav);
//            }

            controllerModel = this.ControllerContext.RouteData.Values["controller"].ToString();
            actionModel = this.ControllerContext.RouteData.Values["action"].ToString();
            var url = Request.Url.PathAndQuery;

            if (User.Identity.IsAuthenticated)
            {
                url = url.Replace(actionModel + "/", "");
                url = url.Replace(controllerModel + "/", "");
                url = url.Substring(url.IndexOf('/') + 1);
                url = url.Substring(url.IndexOf('/') + 1);
                var da = new FormsDataService();

                var activeMenu = da.GetActiveMenuByController(controllerModel);
                if (activeMenu == "")
                {
                    activeMenu = "";
                }

                var side = da.LoadSideBarForms(User.Identity.Name, activeMenu, controllerModel, actionModel, url);
                Menu.Nav = side;
                //                if (!side.Any())
                //                    side = da.LoadSideBarForms(User.Identity.Name, activeMenu);

                //                Menu.SideBarMenu = PartialView("_SideBar", side).RenderToString();
                return PartialView("_SideBar", side);
            }

            return PartialView("_NullView");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using IdentityModel.Principal;
using Microsoft.Owin.Security;
using Services.Helpers;

namespace FitnessCenter.Controllers
{
    [Authorize]
    public class FitController : Controller
    {

        public bool CheckdatestatusAvaliable(DateTime? date)
        {
            return (DateTime.Now.Month.Equals(date?.Month)
                && DateTime.Now.Year.Equals(date?.Year));
        }

        public int? CountDifferentMonthFromNow(DateTime? memberStartdate)
        {
            return ((DateTime.Now.Year - memberStartdate?.Year) * 12) +
                DateTime.Now.Month - memberStartdate?.Month;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public FitController()
        {
            //            if (HttpContext.Session["User"] == null)
            //            {
            //                AuthenticationManager.SignOut();
            //                FormsAuthentication.SignOut();
            //            }
        }
        protected new virtual IdentityPrincipal User
        {
            get { return HttpContext.User as IdentityPrincipal; }
        }

        // GET: Fit
        public void ShowMessage(EnumMessageType messageType, bool dismissable = false)
        {
            if (messageType == EnumMessageType.Danger)
                AddAlert(AlertStyles.Danger, "Data berhasil di <b>HAPUS</b>! Terima Kasih <b>Nama User</b>", dismissable);
            else if (messageType == EnumMessageType.Information)
                AddAlert(AlertStyles.Information, "", dismissable);
            else if (messageType == EnumMessageType.Success)
                AddAlert(AlertStyles.Success, "Data berhasil di <b>SIMPAN</b>! Terima Kasih <b>Nama User</b>", dismissable);
            else if (messageType == EnumMessageType.Warning)
                AddAlert(AlertStyles.Warning, "Data berhasil di <b>EDIT</b>! Terima Kasih <b>Nama User</b>", dismissable);
        }

        public void ShowMessage(EnumMessageType messageType, string message, bool dismissable = false)
        {
            if (messageType == EnumMessageType.Danger)
                AddAlert(AlertStyles.Danger, message, dismissable);
            else if (messageType == EnumMessageType.Information)
                AddAlert(AlertStyles.Information, message, dismissable);
            else if (messageType == EnumMessageType.Success)
                AddAlert(AlertStyles.Success, message, dismissable);
            else if (messageType == EnumMessageType.Warning)
                AddAlert(AlertStyles.Warning, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
    }
}
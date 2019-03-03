using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using FitnessCenter.Binder;
using IdentityModel.Principal;
using Log;

namespace FitnessCenter
{
    public class MvcApplication : HttpApplication
    {
        //private static readonly ILogger Log = LogManager.GetLogger<MvcApplication>();

        protected void Application_Start()
        {

            //            AntiForgeryConfig.SuppressXFrameOptionsHeader =true;


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime?), new DateModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;


            //            AutofacConfig.RegisterComponents();
            //            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            //GetDefaultLocale();

        }

        public string GetDefaultLocale()
        {
            const string localePattern = "~/Scripts/cldr/data/main/{0}"; // where cldr-data lives on disk
            var currentCulture = CultureInfo.CurrentCulture;
            var cultureToUse = "id"; //Default regionalisation to use

            //Try to pick a more appropriate regionalisation
            if (Directory.Exists(HostingEnvironment.MapPath(string.Format(localePattern, currentCulture.Name)))) //First try for a en-GB style directory
                cultureToUse = currentCulture.Name;
            else if (Directory.Exists(HostingEnvironment.MapPath(string.Format(localePattern, currentCulture.TwoLetterISOLanguageName)))) //That failed; now try for a en style directory
                cultureToUse = currentCulture.TwoLetterISOLanguageName;
            return cultureToUse;
        }

        protected void Application_PreSendRequestHeaders()
        {
            //            Response.Headers.Remove("X-Frame-Options");
            //            Response.AddHeader("X-Frame-Options", "AllowAll");

        }

        //        private void Application_Error(object sender, EventArgs e)
        //        {
        //
        //            Debugger.Break();
        //            Exception error = ((HttpApplication)sender).Context.Server.GetLastError();
        //            this.Log().Error("A Fatal unhandled exception has accoured", error);
        //        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    IdentityPrincipalSerializeModel serializeModel = serializer.Deserialize<IdentityPrincipalSerializeModel>(authTicket.UserData);
                    IdentityPrincipal newUser = new IdentityPrincipal(authTicket.Name);
                    newUser.Id = serializeModel.Id;
                    newUser.UserName = serializeModel.UserName;
                    newUser.Person = serializeModel.Person;
                    newUser.Member = serializeModel.Member;
                    newUser.BackOffice = serializeModel.BackOffice;
                    newUser.LocationsId = serializeModel.LocationsId;
                    newUser.ActiveLocation = serializeModel.ActiveLocation;

                    HttpContext.Current.User = newUser;
                }

                catch (Exception ex)
                {
                    FormsAuthentication.SignOut();   
                }
           }
        }
    }
}

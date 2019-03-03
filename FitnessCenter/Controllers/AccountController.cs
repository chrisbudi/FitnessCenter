using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using CrystalDecisions.Shared;
using DataAccessService.Master;
using FitnessCenter.Helper;
using IdentityModel.Config;
using IdentityModel.Model;
using IdentityModel.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Scheme.UOW;
using ViewModel.Master;

namespace FitnessCenter.Controllers
{
    [Authorize]
    public class AccountController : FitController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IServiceMemberMaster _memberManager;
        private IServiceBackOfficeMaster _backOfficeManager;
        private IServiceLocation _locationManager;

        public AccountController(ServiceBackOfficeMaster backOffice,
            ServiceMemberMaster member)
        {
            _backOfficeManager = backOffice;
            _memberManager = member;
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        //
        //        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //        {
        //            UserManager = userManager;
        //            SignInManager = signInManager;
        //        }

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }

        public AccountController()
        {

        }



        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }
        public IServiceMemberMaster MemberManager
        {
            get { return _memberManager ?? new ServiceMemberMaster(new UnitOfWork()); }
            private set { _memberManager = value; }
        }
        public IServiceBackOfficeMaster BackOfficeManager
        {
            get { return _backOfficeManager ?? new ServiceBackOfficeMaster(new UnitOfWork()); }
            private set { _backOfficeManager = value; }
        }
        public IServiceLocation LocManager
        {
            get { return _locationManager ?? new ServiceLocation(new UnitOfWork()); }
            private set { _locationManager = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //            HttpContext.Session["User"] = model.UserName;

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result =
                await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            switch (result)
            {
                case SignInStatus.Success:
                {

                    var currentUser = UserManager.Users.First(u => u.UserName == model.UserName);

                    //var dperson = new DataServicePerson();
                    var member = MemberManager.Get(currentUser.UserName);
                    var backOffice = BackOfficeManager.Get(currentUser.UserName);
                    var person = member != null ? member.tPerson : backOffice.tPerson;
                    var memberIdn = new PersonIdentity.Member();
                    var backOfficeIdn = new PersonIdentity.BackOffice();
                    var personIdentity = new PersonIdentity.Person()
                    {
                        PersonID = person.PersonID,
                        Id = person.Id,
                        PEmail = person.PEmail,
                        PHP1 = person.PHP1,
                        PIdentitas = person.PIdentitas,
                        PNama = person.PNama,
                        PPinBB = person.PPinBB,

                    };
                    if (member != null)
                    {
                        var memberIdentity = new PersonIdentity.Member()
                        {
                            MemberID = member.MemberID,
                            MRFID = member.MRFID,
                            MemberNo = member.MemberNO
                        };
                        memberIdn = memberIdentity;
                    }
                    if (backOffice != null)
                    {
                        var backOfficeIdentity = new PersonIdentity.BackOffice()
                        {
                            BOID = backOffice.BOIDNO,
                            BRFID = backOffice.BRFID,
                            PosisiID = backOffice.PosisiID,
                            StatusBOID = backOffice.StatusBOID
                        };
                        backOfficeIdn = backOfficeIdentity;
                    }
                    var serializeModel = new IdentityPrincipalSerializeModel
                    {
                        Id = currentUser.Id,
                        UserName = currentUser.UserName,
                        Person = personIdentity,
                        Member = memberIdn,
                        BackOffice = backOfficeIdn,
                        LocationsId = "LP",
                        ActiveLocation = LocManager.Get("LP").LocationID
                    };

                    //var locationsId = serializeModel.LocationsId as string[] ?? serializeModel.LocationsId.ToArray();
                    //if (locationsId.Count() == 1)
                    //    serializeModel.ActiveLocation = locationsId.Single();

                    var serializer = new JavaScriptSerializer();

                    var userData = serializer.Serialize(serializeModel);

                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        model.UserName,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(15),
                        false,
                        userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    //                        FormsAuthentication.SetAuthCookie(model.UserName, false);

                    Response.Cookies.Add(faCookie);

                    return Redirect(returnUrl);
                }
                case SignInStatus.LockedOut:
                {
                    return View("Lockout");
                }
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " +
                                 await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result =
                await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, false, model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public async Task CreateBackOffice(CreateVmBackOffice model)
        {
            var user = new ApplicationUser { UserName = model.UserBackOffice.BOIDNO, Password = model.Password, EmailConfirmed = true };
            await RegAsync(user, model.Password);
        }

        public async Task<ApplicationUser> RegAsync(ApplicationUser user, string password)
        {
            await UserManager.CreateAsync(user, password);
            return UserManager.FindByName(user.UserName);
        }


        public ApplicationUser RegisterBo(ApplicationUser user, string password)
        {
            UserManager.Create(user, password);
            return UserManager.FindByName(user.UserName);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CreateVmBackOffice user, string[] SelectedLocation)
        {

            if (ModelState.IsValid)
            {

                var ac = new AccountController(UserManager);
                var dPerson = new DataServicePerson();
                var dBackOffice = new DataServiceUserBackOffice();

                //user.UserBackOffice.BOID = dBackOffice.GetLastNo();
                var applicationUser = new ApplicationUser() { UserName = user.UserBackOffice.BOIDNO, Password = user.Password, EmailConfirmed = true };

                var str = ac.RegAsync(applicationUser, user.Password);
                user.Person.Id = applicationUser.Id;

                dPerson.Insert(user.Person);
                dBackOffice.Insert(user.UserBackOffice);

                //SelectedLocation
                dBackOffice.LocSave(SelectedLocation, user.UserBackOffice.PersonBOID);
                //datase.Crud(model.UserBackOffice, ManageCrud.Create);


                //if (result.Succeeded)
                //{
                //    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                //    ViewBag.Link = callbackUrl;
                //    return View("DisplayEmail");
                //}
                //AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            //return RedirectToAction("Index", "Home");

            return RedirectToAction("index", "UserBackOffice", "Office");

        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code },
                    Request.Url.Scheme);
                await
                    UserManager.SendEmailAsync(user.Id, "Reset Password",
                        "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        //[AllowAnonymous]
        public ActionResult ResetPassword()
        {
            //return code == null ? View("Error") :
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //            if (!ModelState.IsValid)
            //            {
            //                return View(model);
            //            }
            //            var user = await UserManager.FindByNameAsync(model.UserName);
            //            if (user == null)
            //            {
            //                // Don't reveal that the user does not exist
            //                return RedirectToAction("ResetPasswordConfirmation", "Account");
            //            }
            //            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            //            if (result.Succeeded)
            //            {
            //                return RedirectToAction("ResetPasswordConfirmation", "Account");
            //            }
            //            AddErrors(result);
            //            return View();


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);
            if (result.Succeeded)
            {

                var userpass = await UserManager.FindByIdAsync(User.Id);
                userpass.Password = model.Password;
                await UserManager.UpdateAsync(user);

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleAntiForgeryError]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        //
        //        [HttpGet]
        //        [Authorize]
        //        public ActionResult OpenLogOff()
        //        {
        //            AuthenticationManager.SignOut();
        //            FormsAuthentication.SignOut();
        //            return RedirectToAction("Index", "Home");
        //        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        //private IAuthenticationManager IdentityAuthenticationManager
        //{
        //    get { return  HttpContext.GetOwinContext().}
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}
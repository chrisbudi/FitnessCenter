using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using IdentityModel.Config;
using IdentityModel.Model;
using Microsoft.AspNet.Identity.Owin;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Master;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class UserBackOfficeController : FitController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }


        // GET: Master/UserBackOffice
        //[Authorize(Roles="UserBackOffice_R")]
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles="UserBackOffice_C")]
        public ActionResult CreateUser()
        {
            ViewBag.LocationList = new SelectList(new DataServiceLocFitnessCenter().LoadAllData(), "LocationID", "LAlamat");

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(CreateVmBackOffice user, string[] SelectedLocation)
        {
            //            if (!ModelState.IsValid)
            //            {
            //                ViewBag.LocationList = new SelectList(new DataServiceLocFitnessCenter().LoadAllData(), "LAuth", "LocationID");
            //                return View(user);
            //            }

            using (var scope = new TransactionScope())
            {
                try
                {

                    var ac = new AccountController(UserManager);
                    var dPerson = new DataServicePerson();
                    var dBackOffice = new DataServiceUserBackOffice();

                    user.UserBackOffice.BOIDNO = dBackOffice.GetLastNo();

                    var applicationUser = new ApplicationUser()
                    {
                        UserName = user.UserBackOffice.BOIDNO,
                        EmailConfirmed = true
                    };

                    var str = ac.RegisterBo(applicationUser, "123456");

                    user.Person.Id = str.Id;

                    dPerson.Insert(user.Person);
                    user.UserBackOffice.PersonBOID = user.Person.PersonID;

                    dBackOffice.Insert(user.UserBackOffice);
                    dBackOffice.LocSave(SelectedLocation, user.Person.PersonID);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "UserBackOffice_U")]
        public ActionResult EditUser(int id)
        {
            var usr = new DataServiceUserBackOffice().GetobjById(id);
            ViewBag.LocationList = new SelectList(new DataServiceLocFitnessCenter().LoadAllData(), "LAuth", "LocationID");

            EditVmBackOffice bo = new EditVmBackOffice
            {
                UserBackOffice = usr,
                Person = usr.tPerson,
                StrLocBo = usr.strLocBOes
            };
            return View(bo);
        }

        [HttpPost]
        public ActionResult EditUser([Bind(Exclude = "Password")]EditVmBackOffice user, string[] SelectedLocation)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var dPerson = new DataServicePerson();
            var dBackOffice = new DataServiceUserBackOffice();

            //user.Person.Id = str.Result.Id;

            dPerson.Update(user.Person);
            //            user.UserBackOffice.tPerson = null;
            //            user.Person.PersonID = user.Person.PersonID;
            dBackOffice.Update(user.UserBackOffice);
            //SelectedLocation
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "UserBackOffice_D")]
        [HttpPost]
        public ActionResult DeleteUser(tUserBackOffice usr)
        {
            //            if (!ModelState.IsValid)
            //            {
            //                return View("EditUser", usr);
            //            }
            var usrBack = new DataServiceUserBackOffice();
            usrBack.Delete(usr.BOIDNO);
            return RedirectToAction("Index");
        }

        public ActionResult UserBackOfficeResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, FormCollection form)
        {
            var usr = new DataServiceUserBackOffice().LoadData(requestModel);
            int count = requestModel.Start + 1;
            var result = from d in usr.ListClass
                         select new[]
                         {
                             Convert.ToString(count++),
                             d.BOIDNO,
                             d.tPerson.PNama,
                             d.tPosisi.PNamaPosisi,
                             d.StatusBOID == true ? "Aktif" : "Non Aktif",
                             d.tPerson.PGender,
                             d.BMulai.ToString("dd-MM-yyyy"),
                             d.tPerson.PAlamat,
                             d.tPerson.PKota,
                             d.tPerson.PersonID.ToString()
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, usr.TotalFilter,
                usr.Total), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetPT(string searchTerm, int pageSize, int pageNum)
        {
            var ps = new DataServiceUserBackOffice();
            var dpos = new DataServicePosisi();
            var posisi = EnumPosisi.Trainer.GetDescription();

            Debug.Write(dpos.GetPosisiIdByName(posisi));
            List<tUserBackOffice> attendees = ps.GetData(searchTerm, pageSize, pageNum, dpos.GetPosisiIdByName(posisi)).ToList();
            int attendeeCount = ps.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public ActionResult GetInstructur(string searchTerm, int pageSize, int pageNum)
        {
            var ps = new DataServiceUserBackOffice();
            var dpos = new DataServicePosisi();
            var posisi = EnumPosisi.Instructor.ToString("F");
            List<tUserBackOffice> attendees = ps.GetData(searchTerm, pageSize, pageNum, dpos.GetPosisiIdByName(posisi)).ToList();
            int attendeeCount = ps.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult GetUserBoById(string id = "")
        {
            if (id == "")
                return Json(null, JsonRequestBehavior.AllowGet);
            DataServiceUserBackOffice dOffice = new DataServiceUserBackOffice();

            var offices = dOffice.LoadAllData().Where(m => m.BOIDNO == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var office in offices.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = office.PersonBOID.ToString(), text = office.tPerson.PNama });
            }

            jsonAttendees.Total = offices.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetUserBoByPersonId(int id = 0)
        {
            if (id == 0)
                return Json(null, JsonRequestBehavior.AllowGet);
            DataServiceUserBackOffice dOffice = new DataServiceUserBackOffice();

            var offices = dOffice.LoadAllData().Where(m => m.tPerson.PersonID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var office in offices.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = office.PersonBOID.ToString(), text = office.tPerson.PNama });
            }

            jsonAttendees.Total = offices.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        private Select2PagedResult AttendeesToSelect2Format(List<tUserBackOffice> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
                jsonAttendees.Results.Add(new Select2StringResult { id = a.PersonBOID.ToString(), text = a.tPerson.PNama });

            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }

        [HttpPost]
        public ActionResult GetListSalesName(string searchTerm, int pageSize, int pageNum)
        {
            var ps = new DataServiceUserBackOffice();
            var dpos = new DataServicePosisi();
            List<tUserBackOffice> attendees = ps.GetData(searchTerm, pageSize, pageNum, dpos.GetPosisiIdByName("Fitness Consultant")).ToList();
            int attendeeCount = ps.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetListSalesNameById(string id = "")
        {
            if (id == "")
                return Json(null);

            DataServiceUserBackOffice dBackOffice = new DataServiceUserBackOffice();

            var backOffice = dBackOffice.LoadAllData().Where(m => m.BOIDNO == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var bo in backOffice.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = bo.BOIDNO, text = bo.tPerson.PNama });
            }

            jsonAttendees.Total = backOffice.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
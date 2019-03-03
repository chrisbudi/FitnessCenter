using System;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Activity;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.DataTables;

namespace FitnessCenter.Areas.Activity.Controllers
{
    public class CheckInOutBOController : FitController
    {
        // GET: Registrasi/CheckInOutBO
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id, FormCollection frm)
        {
            tUserBackOffice validBOID = new DataServiceUserBackOffice().GetobjById(id);
            if (!string.IsNullOrEmpty(validBOID.BOIDNO))
            {
                int cio = new DataServiceCheckInOutBO().GetLastStatusID(id);
                int cin = new DataServiceTypeStatusCinCoutBO().GetStatusId("Check Out");
                if (cio == cin)
                    return RedirectToAction("CheckIn", new { BOID = id });
                else
                    return RedirectToAction("CheckOut", new { BOID = id });
            }
            else
            { return View(); }
        }

        public ActionResult CheckIn(int id)
        {
            var aktifitas = new trCinCout
            {
                PersonBOID = id,
                TimeStatus = DateTime.Now,
                LocBoID = new DataServiceCheckInOutBO().GetLocBoID(User.ActiveLocation, User.Person.PersonID),
                TypeStatusInOut = new DataServiceTypeStatusCinCoutBO().GetStatusId("Check In")
            };
            return View(aktifitas);
        }

        [HttpPost]
        public ActionResult CheckIn(trCinCout akt)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var aktifitas = new DataServiceCheckInOutBO();
            aktifitas.InsertCheckIn(akt);
            return RedirectToAction("Index");
        }

        public ActionResult CheckOut(string id)
        {
            var aktifitas = new trCinCout
            {
                TimeStatus = DateTime.Now,
                PersonBOID = User.Person.PersonID,
                LocBoID = new DataServiceCheckInOutBO().GetLocBoID(User.ActiveLocation, User.Person.PersonID),
                TypeStatusInOut = new DataServiceTypeStatusCinCoutBO().GetStatusId("Check Out")
            };
            return View(aktifitas);
        }

        [HttpPost]
        public ActionResult CheckOut(trCinCout akt)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var aktifitas = new DataServiceCheckInOutBO();
            aktifitas.InsertCheckOut(akt);
            return RedirectToAction("Index");
        }

        public ActionResult CIOResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cio = new DataServiceCheckInOutBO().LoadData(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cio.ListClass
                         orderby d.TimeStatus descending
                         select new object[]
                {
                    Convert.ToString(count++),
                    d.tUserBackOffice.BOIDNO,
                    d.TimeStatus.ToString("dd-MM-yyyy hh:mm:ss tt"),
                    d.strLocBO.tLocFitnessCenter.LAuth,
                    d.tTypeStatusCinCout.NameStatusInOut
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cio.TotalFilter,
                cio.Total), JsonRequestBehavior.AllowGet);
        }
    }
}
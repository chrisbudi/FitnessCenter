using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using FitnessCenter.Models.Crystal_Report;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Membership.Registrasi;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class MemberController : FitController
    {
        // GET: Master/Member
        //[Authorize(Roles="Member_R")]
        public ActionResult Index()
        {
            //string str = User.Identity.Name;

            return View();
        }

        //[Authorize(Roles = "Member_U")]
        public ActionResult EditMember(int id)
        {
            var member = new DataServiceMember().GetobjByPersonId(id);
            ////ViewData[]
            return View(member);
        }


        [HttpPost]
        public ActionResult EditMember([Bind(Exclude = "tMemberType")]tMember mem, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(mem);
            }

            mem.tMemberType = null;

            var member = new DataServiceMember();
            member.Update(mem);
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Member_D")]
        [HttpPost]
        public ActionResult DeleteMember(tMember mem)
        {
            if (!ModelState.IsValid)
            {
                return View("EditMember", mem);
            }

            var member = new DataServiceMember();
            member.Delete(mem.MemberID);
            return RedirectToAction("Index");
        }



        public CrystalReportPdfResult PrintOr(int id)
        {
            string reportPath = Server.MapPath(@"~\bin\Areas\Office\Views\Member\Report\PrintoutQR.rpt");
            var dMembership = new DataServiceMembership();

            var reportSource = (from m in dMembership.LoadAllData().Where(m => m.trMembershipID == id).ToList()
                                select new PrintoutQr()
                                {
                                    MemberType = m.tMember.tMemberType.MemberType,
                                    Nama = m.tMember.tPerson.PNama,
                                    Alamat = m.tMember.tPerson.PNama,
                                    TanggalMulai = m.MSTglMulai
                                }).First();

            return new CrystalReportPdfResult(reportPath, new[] { reportSource }, null);
        }

        public ActionResult LoadFotoMember(int id)
        {
            string url = Request.Url.Query;
            var dMember = new DataServiceMember();
            var imageData = dMember.LoadAllData().Single(m => m.MemberID == id);

            return File(imageData.MFoto, "image/jpg");
        }

        public ActionResult MemberResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var member = new DataServiceMember().LoadData(requestModel, User.ActiveLocation);
            int count = requestModel.Start + 1;
            var result = from d in member.ListClass
                             //                         let tgllahir = d.tMember.tPerson.PTglLahir
                             //                         where tgllahir != null
                         select new[]
                {
                    Convert.ToString(count++),
                    d.MemberNO,
                    d.tPerson.PNama,
                    d.tPerson.PAlamat,
                    d.tPerson.PKota,
                    d.tPerson.PTglLahir == null ?
                    "1900-01-01" : d.tPerson.PTglLahir.Value.ToString("dd-MM-yyyy"),
                    d.tPerson.PGender,
                    d.trMemberships.First(m => m.tMember.tPerson.PersonID == d.tPerson.PersonID).tStatusMember.STKet,
                    d.trMemberships.First(m => m.tMember.tPerson.PersonID == d.tPerson.PersonID).ActivationCode,
                    d.PersonID.ToString()
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, member.TotalFilter,
                member.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MemberCardResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string memberType)
        {
            if (memberType == null)
            {
                memberType = "New";
            }

            var member = new DataServiceMember().LoadData(requestModel, User.ActiveLocation, memberType);

            int count = requestModel.Start + 1;
            var result = from d in member.ListClass
                         select new[]
                {
                    Convert.ToString(count++),
                    d.tMember.MemberNO,
                    d.tMember.tPerson.PNama,
                    d.tMember.PersonID.ToString()
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, member.TotalFilter,
                member.Total), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMember(string searchTerm, int pageSize, int pageNum)
        {
            var ps = new DataServiceMember();
            List<tMember> attendees = ps.GetData(searchTerm, pageSize, pageNum).ToList();
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
        public ActionResult GetMemberByid(int id = 0)
        {
            if (id == 0)
                return Json(null, JsonRequestBehavior.AllowGet);
            DataServiceMember dOffice = new DataServiceMember();

            var offices = dOffice.LoadAllData().Where(m => m.MemberID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var office in offices.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = office.MemberID.ToString(), text = office.tPerson.PNama + " - " + office.tPerson.PAlamat });
            }

            jsonAttendees.Total = offices.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(List<tMember> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.MemberID.ToString(CultureInfo.InvariantCulture), text = a.tPerson.PNama });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }
}
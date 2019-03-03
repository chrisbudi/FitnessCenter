using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using Services.Helpers;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class PaketPTController : FitController
    {
        // GET: Master/PaketPT
        //[Authorize(Roles="PaketPT_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsPktPT(int id)
        {
            tPaketPT pkt = new DataServicePaketPT().GetobjById(id);
            return View(pkt);
        }

        //[Authorize(Roles="PaketPT_C")]
        public ActionResult CreatePktPT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePktPT(tPaketPT pkt)
        {
            if (!ModelState.IsValid)
            {
                return View(pkt);
            }
            var pktPt = new DataServicePaketPT();
            pktPt.Insert(pkt);
            return RedirectToAction("DetailsPktPT", new { id = pkt.tPaketPTID });
        }

        //[Authorize(Roles = "PaketPT_U")]
        public ActionResult EditPktPT(int id)
        {
            var pkt = new DataServicePaketPT().GetobjById(id);
            return View(pkt);
        }

        [HttpPost]
        public ActionResult EditPktPT(tPaketPT pkt, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(pkt);
            }

            var pktPt = new DataServicePaketPT();
            pktPt.Update(pkt);
            return RedirectToAction("DetailsPktPT", new { id = pkt.tPaketPTID });
        }

        //[Authorize(Roles = "PaketPT_D")]
        [HttpPost]
        public ActionResult DeletePktPT(tPaketPT pkt)
        {
            if (!ModelState.IsValid)
            {
                return View("EditPktPT", pkt);
            }

            var pktPt = new DataServicePaketPT();
            pktPt.Delete(pkt.tPaketPTID);
            return RedirectToAction("Index");
        }

        public ActionResult PaketPTResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tPaketPT> loadPkt = new DataServicePaketPT().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadPkt.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.tPaketPTID,
                             d.PPTNama,
                             d.PPTJam,
                             d.PPTHarga?.ToString("N"),
                             d.PPTMasa
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadPkt.TotalFilter,
                loadPkt.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPaketPT(string searchTerm, int pageSize, int pageNum, bool forMembership)
        {
            var ar = new DataServicePaketPT();
            var attendees = ar.GetData(searchTerm, pageSize, pageNum, forMembership);
            int attendeeCount = ar.GetDataCount(searchTerm);

            var pagedAttendees = PaketPTToSelect2Format(attendees, attendeeCount);

            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private object PaketPTToSelect2Format(IEnumerable<tPaketPT> attendees, int attendeeCount)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.tPaketPTID.ToString(), text = a.PPTNama });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = attendeeCount;

            return jsonAttendees;
        }


        public ActionResult GetPaketPTById(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServicePaketPT dPkt = new DataServicePaketPT();

            var types = dPkt.LoadAllData().Where(m => m.tPaketPTID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var action in types.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.tPaketPTID.ToString(), text = action.PPTNama });
            }

            jsonAttendees.Total = types.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetPaketPTSharedById(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServicePaketPT dPkt = new DataServicePaketPT();

            var packets = dPkt.GetobjById(id);
            var packet = new tPaketPT()
            {
                tPaketPTID = packets.tPaketPTID,
                PPTNama = packets.PPTNama,
                PPTMasa = packets.PPTMasa,
                PPTJam = packets.PPTJam,
                PPTHarga = packets.PPTHarga,
                PPTPersonTotal = packets.PPTPersonTotal,
                PPTStatus = packets.PPTStatus
            };

            return new Jsonp()
            {
                Data = packet,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public ActionResult GetPersonalTrainerID(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServiceMemberType dType = new DataServiceMemberType();

            var types = dType.LoadAllData().Where(m => m.MemberTypeID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var action in types.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.MemberTypeID.ToString(), text = action.MemberType });
            }

            jsonAttendees.Total = types.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetPaketPTID(int id = 0)
        {
            if (id == 0)
                return Json(null);

            var ars = new DataServicePaketPT().LoadAllData().Where(m => m.tPaketPTID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var action in ars.ToList())
            {
                jsonAttendees.Results.Add(new Select2StringResult
                {
                    id = action.tPaketPTID.ToString(),
                    text = $"{action.tPaketPTID} - {action.PPTNama}"
                });
            }

            jsonAttendees.Total = ars.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetPaketPTList(string searchTerm, int pageSize, int pageNum)
        {
            var ar = new DataServicePaketPT();
            var type = ar.GetData(searchTerm, pageSize, pageNum, true);
            int typeCount = ar.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = PersonalTrainerToSelect2Format(type, typeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private object PersonalTrainerToSelect2Format(IEnumerable<tPaketPT> attendees, int attendeeCount)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.tPaketPTID.ToString(), text = a.PPTNama });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = attendeeCount;

            return jsonAttendees;
        }


    }
}
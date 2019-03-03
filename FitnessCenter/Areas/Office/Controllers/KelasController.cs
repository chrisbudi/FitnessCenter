using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Instruktur;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using Services.Helpers;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class KelasController : FitController
    {
        // GET: Master/Kelas
        //[Authorize(Roles="Kelas_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetKelas()
        {
            DataServiceKelas druang = new DataServiceKelas();

            var actions = druang.LoadAllData();

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            var tSalesActions = actions as IList<tKela> ?? actions.ToList();
            foreach (var action in tSalesActions.ToList())
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = action.KelasID.ToString(), text = action.KNamaKelas });
            }
            jsonAttendees.Total = tSalesActions.Count();
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetKelasById(int id = 0)
        {
            if (id == 0)
                return Json(null, JsonRequestBehavior.AllowGet);

            DataServiceKelas dOffice = new DataServiceKelas();

            var offices = dOffice.LoadAllData().Where(m => m.KelasID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var office in offices.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = office.KelasID.ToString(), text = office.KNamaKelas });
            }

            jsonAttendees.Total = offices.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetMemberTypeById(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServiceKelas dkelas = new DataServiceKelas();

            var kelas = dkelas.LoadAllData().Where(m => m.KelasID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var kela in kelas.ToList())
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = kela.KelasID.ToString(), text = kela.KNamaKelas });
            }

            jsonAttendees.Total = kelas.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult DetailsKelas(int id)
        {
            var kls = new DataServiceKelas().GetobjByID(id);
            return View(kls);
        }

        //[Authorize(Roles="Kelas_C")]
        public ActionResult CreateKelas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateKelas(tKela kls)
        {
            if (!ModelState.IsValid)
            {
                return View(kls);
            }
            var file = Request.Files["ImageData"];
            var service = new DataServiceKelas();
            //            int i = 
            service.Insert(file, kls);

            return RedirectToAction("DetailsKelas", new { id = kls.KelasID });
        }

        //[Authorize(Roles = "Kelas_U")]
        public ActionResult EditKelas(int id)
        {
            var kls = new DataServiceKelas().GetobjByID(id);
            return View(kls);
        }

        [HttpPost]
        public ActionResult EditKelas(tKela kls, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(kls);
            }

            var file = Request.Files["ImageData"];
            var service = new DataServiceKelas();
            service.Update(file, kls);
            return RedirectToAction("DetailsKelas", new { id = kls.KelasID });
        }

        //[Authorize(Roles = "Kelas_D")]
        [HttpPost]
        public ActionResult DeleteKelas(tKela kls)
        {
            if (!ModelState.IsValid)
            {
                return View("EditKelas", kls);
            }
            var kelas = new DataServiceKelas();
            kelas.Delete(kls.KelasID);
            return RedirectToAction("Index");
        }


        /// <summary>
        ///     Retrive Image from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RetrieveImage(int id)
        {
            var cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] GetImageFromDataBase(int id)
        {
            var service = new DataServiceKelas();
            var q = service.LoadAllData().Single(m => m.KelasID == id).ImageKelas;
            var cover = q;
            return cover;
        }


        public ActionResult KelasResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var loadKls = new DataServiceKelas().LoadData(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in loadKls.ListClass
                         select new object[]
                         {
                    Convert.ToString(count++),
                    d.KNamaKelas,
//                             d.KKapasitas,
                    d.KelasID
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadKls.TotalFilter,
                loadKls.Total), JsonRequestBehavior.AllowGet);
        }
    }
}
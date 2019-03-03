using FitnessCenter.Controllers;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using DataObjects.Context;
using DataObjects.Entities;


namespace FitnessCenter.Areas.Event.Controllers
{
    public class FBCController : FitController
    {
        private FitEntity db = new FitEntity();
        // GET: Event/FBC
        public ActionResult Index(int? page)
        {
            //page = 1;

            var ev = (from s in db.trEventSteps
                      select s).AsEnumerable().Select(m => new trEventScore()
                      {
                          Score = 0,
                          trEventStep = m,
                          tEventScore = m.tEventStep.tEventScores.OrderBy(s => s.EvScoreId).First()
                      });

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(ev.OrderBy(m => m.trEventStep.trPersonEvent.strPersonEvent.NoPeserta).ToPagedList(pageNumber, pageSize));
            //return View();
        }
    }
}
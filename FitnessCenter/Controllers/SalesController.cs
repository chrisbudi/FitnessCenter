using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataObjects.Entities;
using Services.Class;
using Services.Helpers;

namespace FitnessCenter.Controllers
{
    public class SalesController : FitController
    {
        // GET: General/Sales
        public ActionResult GetSalesAction()
        {
            DataServiceSalesAction dAction = new DataServiceSalesAction();

            var actions = dAction.LoadAllData();

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            var tSalesActions = actions as IList<tSalesAction> ?? actions.ToList();
            foreach (var action in tSalesActions.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.SalesActionID.ToString(), text = action.ActionName });
            }
            jsonAttendees.Total = tSalesActions.Count();
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetSalesActionById(int id = 0)
        {

            if (id == 0)
                return Json(null);
            var dAction = new DataServiceSalesAction();

            var actions = dAction.LoadAllData().Where(m => m.SalesActionID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            var tSalesActions = actions as IList<tSalesAction> ?? actions.ToList();
            foreach (var action in tSalesActions.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.SalesActionID.ToString(), text = action.ActionName });
            }
            jsonAttendees.Total = tSalesActions.Count();
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetSalesStatus()
        {
            DataServiceMemberState dState = new DataServiceMemberState();

            var statuses = dState.LoadAllData();

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            var tMemberStates = statuses as IList<tMemberState> ?? statuses.ToList();

            foreach (var status in tMemberStates.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = status.MemberStateID.ToString(), text = status.MemberStateName });
            }

            jsonAttendees.Total = tMemberStates.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetSalesStatusById(int id = 0)
        {
            if (id == 0)
                return Json(null);
            DataServiceMemberState dState = new DataServiceMemberState();

            var statuses = dState.LoadAllData().Where(m => m.MemberStateID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var status in statuses.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = status.MemberStateID.ToString(), text = status.MemberStateName });
            }

            jsonAttendees.Total = statuses.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
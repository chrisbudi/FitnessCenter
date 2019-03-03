using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Entities;
using Services.Class;
using Services.DataTables;

//using Fit.Membership.Model;

namespace DataAccessService.Activity
{
    public class DataServiceCheckInOutBO : DbDataAccessActivity
    {
        public IQueryable<trCinCout> aktBO { get; set; }

        public Counter<trCinCout> LoadData(IDataTablesRequest request)
        {
            IQueryable<trCinCout> act = (from p in _db.trCinCouts
                                         select p);

            int actCounter = act.Count();

            IQueryable<trCinCout> filteredAct = (from e in act
                                                 where
                                                 e.tUserBackOffice.BOIDNO.Contains(request.Search.Value)
                                                 select e);

            List<trCinCout> paged =
                filteredAct.OrderBy(x => x.CinCoutID)
                    .Skip(request.Start)
                    .Take(request.Length)
                    .ToList();

            return new Counter<trCinCout> { ListClass = paged.ToList(), Total = actCounter, TotalFilter = filteredAct.Count() };
        }

        public int GetLastStatusID(int BOID)
        {
            int act = (from p in _db.trCinCouts
                       where p.PersonBOID == BOID
                       orderby p.TimeStatus descending
                       select p.TypeStatusInOut).First();
            return act;
        }

        public int GetLocBoID(int locID, int idBO)
        {
            int loc = (from p in _db.strLocBOes
                       where p.LocationID == locID &&
                       p.PersonBOID == idBO
                       select p.LocBoID).Single();
            return loc;
        }

        public trCinCout GetobjById(int boid)
        {
            var act = (from p in _db.trCinCouts
                       where p.PersonBOID == boid
                       orderby p.TimeStatus descending
                       select p).SingleOrDefault();
            return act;
        }


        public void InsertCheckIn(trCinCout obj)
        {
            _db.trCinCouts.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void InsertCheckOut(trCinCout obj)
        {
            _db.trCinCouts.Add(obj);
            Save();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using DataObjects.Entities;
using Services.Class;
using Services.DataTables;
using Services.Helpers;


namespace DataAccessService.Activity
{
    public class DataServiceCheckInOut : DbDataAccessActivity
    {
        public IQueryable<trAktifitasMember> AktMember { get; set; }

        public Counter<trAktifitasMember> LoadData(IDataTablesRequest request)
        {

            var datetimeNow = DateTime.Now.Date;
            IQueryable<trAktifitasMember> act = (from p in _db.trAktifitasMembers
                                                 where DbFunctions.TruncateTime(p.AMMulai) == datetimeNow
                                                 select p);

            int actCounter = act.Count();


            IQueryable<trAktifitasMember> filteredAct = (from e in act
                                                         where
                                                         e.tUserBackOffice.BOIDNO.Contains(request.Search.Value) 
                                                         select e);

            List<trAktifitasMember> paged =
                filteredAct.OrderBy(x => x.AktifitasMemberID)
                    .Skip(request.Start)
                    .Take(request.Length)
                    .ToList();

            return new Counter<trAktifitasMember> { ListClass = paged.ToList(), Total = actCounter, TotalFilter = filteredAct.Count() };
        }

        public int GetObjCountByMemberID(int memberID)
        {
            IQueryable<trAktifitasMember> act = (from p in _db.trAktifitasMembers
                                                 where p.tMember.MemberID == memberID
                                                 && p.AMMulai != null
                                                 && p.AMSelesai == null
                                                 && p.Status == "In"
                                                 orderby p.AktifitasMemberID descending
                                                 select p);
            int actCounter = act.Count();
            return actCounter;
        }

        public trAktifitasMember GetobjByMemberId(int memberID)
        {
            var stat = "In";
            var act = (from p in _db.trAktifitasMembers
                       orderby p.AktifitasMemberID descending
                       select p).First();
            act.AMSelesai = DateTime.Now;
            return act;
        }

        public trAktifitasMember GetobjById(int Id)
        {
            var act = (from p in _db.trAktifitasMembers
                       where p.AktifitasMemberID == Id
                       select p).SingleOrDefault();
            return act;
        }

        public void InsertCheckIn(trAktifitasMember obj)
        {
            _db.trAktifitasMembers.Add(obj);
            Save();
        }

        public void UpdateCheckOut(trAktifitasMember obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<trAktifitasMember> LoadAllData()
        {
            return _db.trAktifitasMembers;
        }

        public Counter<trAktifitasMember> LoadHistoryData(IDataTablesRequest requestModel, int memberid)
        {
            IQueryable<trAktifitasMember> act = (from p in _db.trAktifitasMembers
                                                 select p);

            act = act.Where(m => m.MemberID == memberid);

            int actCounter = act.Count();

            IQueryable<trAktifitasMember> filteredAct = (from e in act
                                                         select e);

            List<trAktifitasMember> paged =
                filteredAct.OrderByDescending(x => x.AktifitasMemberID)
                    .Skip(0)
                    .Take(5)
                    .ToList();

            return new Counter<trAktifitasMember> { ListClass = paged.ToList(), Total = actCounter, TotalFilter = filteredAct.Count() };
        }
    }
}

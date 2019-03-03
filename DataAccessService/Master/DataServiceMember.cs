using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataObjects.Entities;
using DataObjects.Shared;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;

namespace DataAccessService.Master
{
    public class DataServiceMember : DbDataAccessMaster
    {
        //private readonly fitnessDBEntities Sp = new fitnessDBEntities();

        public IQueryable<tMember> Members = null;

        public string GetLastNo(string location, string extensi)
        {
            var formatNo = location;
            var formatExtensi = extensi;
            return "";
            //return Sp.GetLastMemberId(formatNo, formatExtensi).First();
        }


        public IQueryable<tMember> LoadAllData()
        {
            return from p in DbMaster.tMembers
                       //where p.tr
                   select p;
        }


        public Counter<tMember> LoadData(IDataTablesRequest request, int location)
        {
            var statusMemberId = new DataServiceStatusMember().GetStatusId(EnumStatusMember.Membership);

            var membership = from p in DbMaster.tMembers
                             where p.trMemberships.Any(m => m.StatusMID == statusMemberId)
                             select p;
            //.Where(m => m.StatusMID == statusMemberId);

            var member = membership;

            var memberCounter = member.Count();
            var filteredMember = member;

            filteredMember = request.Columns.
                Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(filteredMember, (current, column) =>
                current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tMember> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredMember.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredMember.OrderBy(m => m.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            return new Counter<tMember>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }

        public tMember GetobjByPersonId(int Id)
        {
            var member = (from p in DbMaster.tMembers
                          where p.PersonID == Id
                          select p).SingleOrDefault();
            return member;
        }

        public void Insert(tMember obj)
        {
            //throw new NotImplementedException();
            DbMaster.tMembers.Add(obj);
            Save();
        }

        //public void InsertLocation(strLocMember obj)
        //{
        //    //throw new NotImplementedException();
        //    DbMembership.strLocMembers.Add(obj);
        //    Save();
        //}

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }


        public void Update(tMember obj)
        {
            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public tMember GetobjByMemberId(int id)
        {
            return DbMaster.tMembers.
                SingleOrDefault(m => m.MemberID == id);
        }


        public Counter<trMembership> LoadData(IDataTablesRequest request, int activeLocation, string memberType = "")
        {
            var statusMemberId = new DataServiceStatusMember().GetStatusId(EnumStatusMember.Membership);
            var membership = from p in DbMaster.trMemberships
                             where p.StatusMID == statusMemberId
                             select p;

            membership = memberType.ToUpper() == "NEW" ?
                membership.Where(m => m.CardStatus == 2 && m.tCardStatu.FinalStatus == false) :
                membership.Where(m => m.tMember.tMemberType.MemberType.Contains(memberType) && m.tCardStatu.FinalStatus);

            var member = membership;

            var memberCounter = member.Count();
            var filteredMember = member;

            filteredMember = request.Columns.
                Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(filteredMember, (current, column) =>
                current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredMember.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredMember.OrderBy(m => m.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            var pag = paged.ToList();
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }

        public List<tMember> GetData(string searchTerm, int pageSize, int pageNum)
        {
            Members = LoadAllData().AsQueryable();
            return GetDataQuery(searchTerm).OrderBy(m => m.MemberID)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();

        }

        public IQueryable<tMember> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return Members.Where(
                    a => a.tPerson.PNama.Contains(searchTerm)
                );
        }

        public int GetDataCount(string searchTerm)
        {
            return Members.Count();
        }
    }
}
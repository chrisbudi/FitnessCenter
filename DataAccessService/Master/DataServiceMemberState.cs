using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Entities;
using DataObjects.Shared;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;

namespace DataAccessService.Master
{
    public class DataServiceMemberState : DbDataAccessMaster
    {

        public DataServiceMemberState() { }

        private IQueryable<tMemberState> _memberStates { get; set; }

        public IQueryable<tMemberState> LoadAllData()
        {
            _memberStates = (from p in DbMaster.tMemberStates
                             select p);
            return _memberStates;
        }

        public Counter<tMemberState> LoadData(IDataTablesRequest request)
        {


            var stat = (from p in DbMaster.tMemberStates
                        select p);

            var memberCounter = stat.Count();
            var filteredMember = stat;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredMember = filteredMember.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tMemberState>(listFilter).Compile();
                filteredMember = filteredMember.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tMemberState> paged;

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
                    filteredMember.OrderBy(m => m.MemberStateID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tMemberState>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }

        public tMemberState GetobjById(int Id)
        {
            var member = (from p in DbMaster.tMemberStates
                          where p.MemberStateID == Id
                          select p).SingleOrDefault();
            return member;
        }

        public int GetMemberstateId(EnumMemberState state)
        {
            var statedString = state.ToString("F");
            return DbMaster.tMemberStates.Single(m => m.MemberStateName == statedString).MemberStateID;
        }


        public void Insert(tMemberState obj)
        {
            DbMaster.tMemberStates.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tMemberState obj)
        {
            var updatedData = GetobjById(obj.MemberStateID);
            updatedData.MemberStateName = obj.MemberStateName;
            updatedData.Note = obj.Note;
            Save();
        }
    }
}
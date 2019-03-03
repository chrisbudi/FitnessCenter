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
    public class DataServiceStatusMember : DbDataAccessMaster
    {
        public IQueryable<tStatusMember> LoadAllData()
        {
            return DbMaster.tStatusMembers;
        }

        public tStatusMember GetobjById(int Id)
        {
            var status = (from p in DbMaster.tStatusMembers
                          where p.StatusMID == Id
                          select p).SingleOrDefault();
            return status;
        }

        public IEnumerable<string> StatusActionsLoad()
        {
            return Enum.GetValues(typeof(EnumModuleForm)).Cast<EnumModuleForm>().Select(m => m.ToString()).ToList();
        }

        public void Insert(tStatusMember obj)
        {
            DbMaster.tStatusMembers.Add(obj);
            Save();
        }

        public void InsertDetail(tStatusMemberPrice obj)
        {
            DbMaster.tStatusMemberPrices.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tStatusMember obj)
        {
            var updatedData = GetobjById(obj.StatusMID);
            updatedData.STKet = obj.STKet;
            Save();
        }

        public Counter<tStatusMember> LoadData(IDataTablesRequest request)
        {
            var stat = (from p in DbMaster.tStatusMembers
                        where !p.tStatusMemberPrices.Any()
                        select p);

            var statCounter = stat.Count();
            var filtered = stat;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filtered = filtered.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tStatusMember>(listFilter).Compile();
                filtered = filtered.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tStatusMember> paged;

            if (ord.Orderable)
            {
                paged =
                    filtered.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filtered.OrderBy(m => m.StatusMID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tStatusMember>
            {
                ListClass = paged.ToList(),
                Total = statCounter,
                TotalFilter = filtered.Count()
            };
        }

        public Counter<tStatusMember> LoadDataPayment(IDataTablesRequest request)
        {
            var stat = (from p in DbMaster.tStatusMembers
                        where p.tStatusMemberPrices.Any()
                        select p).Distinct();

            var statCounter = stat.Count();
            var filtered = stat;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filtered = filtered.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tStatusMember>(listFilter).Compile();
                filtered = filtered.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tStatusMember> paged;

            if (ord.Orderable)
            {
                paged =
                    filtered.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filtered.OrderByDescending(m => m.STKet)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tStatusMember>
            {
                ListClass = paged.ToList(),
                Total = statCounter,
                TotalFilter = filtered.Count()
            };
        }

        public IEnumerable<EnumStatusRegister> LoadAllStatusRegister()
        {
            return Enum.GetValues(typeof(EnumStatusRegister)).Cast<EnumStatusRegister>();
        }

        public int GetStatusId(EnumStatusRegister memberStatus)
        {
            var status = memberStatus.GetDescription();
            return LoadStatusId(status);
        }

        public int GetStatusId(EnumStatusMember memberStatus)
        {
            var status = memberStatus.GetDescription();
            return LoadStatusId(status);
        }

        private int LoadStatusId(string status)
        {
            return (from p in DbMaster.tStatusMembers
                    where p.STKet == status
                    select p).OrderByDescending(m => m.STKet).First().StatusMID;
        }

        public int GetStatusActionId(string status)
        {
            var actionId = (from p in DbMaster.tStatusMembers
                            where p.STKet == status
                            select p).OrderByDescending(m => m.STKet).First().StatusMID;

            return actionId;
        }


        public void UpdateDetail(tStatusMemberPrice tStatusMemberPrice)
        {
            throw new NotImplementedException();
        }
    }
}
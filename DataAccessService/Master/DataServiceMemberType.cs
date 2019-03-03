using DataObjects.Entities;
using DataObjects.Shared;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessService.Master
{
    public class DataServiceMemberType : DbDataAccessMaster
    {

        private IQueryable<tMemberType> _type;

        public IQueryable<tMemberType> LoadAllData()
        {
            //_type = (from p in DbMaster.tMemberTypes
            //         select p);
            //return _type;

            return DbMaster.tMemberTypes;
        }


        public Counter<tMemberType> LoadData(IDataTablesRequest request)
        {
            //_type = LoadAllData();
            //int memberCounter = _type.Count();

            //var filteredMember = (from e in _type
            //                      where
            //                      e.MemberType.Contains(request.Search.Value)
            //                      select e);

            //List<tMemberType> paged =
            //    filteredMember.OrderBy(x => x.MemberTypeID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            var tipe = (from p in DbMaster.tMemberTypes
                        select p);

            var memberCounter = tipe.Count();
            var filteredMember = tipe;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredMember = filteredMember.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }


            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tMemberType> paged;

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
                    filteredMember.OrderBy(m => m.MemberTypeID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tMemberType>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }


        public tMemberType GetobjById(int? id)
        {
            var member = (from p in DbMaster.tMemberTypes
                          where p.MemberTypeID == id
                          select p).SingleOrDefault();
            return member;
        }

        public tMemberType GetobjByMemberType(string memberType)
        {
            var member = (from p in DbMaster.tMemberTypes
                          where p.MemberType == memberType
                          select p).SingleOrDefault();
            return member;
        }

        public void Insert(tMemberType obj)
        {
            DbMaster.tMemberTypes.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tMemberType obj)
        {
            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<tMemberType> GetData(string searchTerm, int pageSize, int pageNum)
        {
            _type = LoadAllData();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize);
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }


        public IEnumerable<tMemberType> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return _type.Where(a => a.MemberType.Contains(searchTerm));
        }


    }
}

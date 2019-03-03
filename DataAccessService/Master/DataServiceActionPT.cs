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
    public class DataServiceActionPT : DbDataAccessMaster
    {
        private IEnumerable<tActionPT> _act;

        public IEnumerable<tActionPT> LoadAllData()
        {
            _act = (from p in DbMaster.tActionPTs
                    select p);

            return _act;
        }

        public Counter<tActionPT> LoadData(IDataTablesRequest request)
        {
            //LoadAllData();

            //int actCounter = _act.Count();

            //var filteredAct = (from e in _act
            //                   where
            //                   e.ActionPTName.Contains(request.Search.Value)
            //                   select e);

            //List<tActionPT> paged =
            //    filteredAct.OrderBy(x => x.ActionPTID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tActionPT> { ListClass = paged.ToList(), Total = actCounter, TotalFilter = filteredAct.Count() };

            var act = (from p in DbMaster.tActionPTs
                       select p);

            var actCounter = act.Count();
            var filteredAct = act;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    
                    filteredAct = filteredAct.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }


            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tActionPT> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredAct.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredAct.OrderBy(m => m.ActionPTID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tActionPT>
            {
                ListClass = paged.ToList(),
                Total = actCounter,
                TotalFilter = filteredAct.Count()
            };
        }


        public tActionPT GetobjByID(int Id)
        {
            var act = (from p in DbMaster.tActionPTs
                       where p.ActionPTID == Id
                       select p).SingleOrDefault();
            return act;
        }

        public void Insert(tActionPT obj)
        {
            DbMaster.tActionPTs.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tActionPT obj)
        {
            tActionPT updatedData = GetobjByID((int)obj.ActionPTID);
            updatedData.ActionPTName = obj.ActionPTName;
            updatedData.ActionPTKet = obj.ActionPTKet;
            Save();
        }

        public List<tActionPT> LoadDataForDropdownlist()
        {
            List<tActionPT> act = (from p in DbMaster.tActionPTs
                                         orderby p.ActionPTID
                                         select p).ToList();
            return act;
        }

        public List<tActionPT> GetData(string searchTerm, int pageSize, int pageNum)
        {
            _act = LoadDataForDropdownlist().AsQueryable();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IEnumerable<tActionPT> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return _act.Where(
                    a => a.ActionPTName.Like(searchTerm) 
                );
        }
    }
}

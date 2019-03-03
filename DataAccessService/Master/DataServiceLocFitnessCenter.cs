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
    public class DataServiceLocFitnessCenter : DbDataAccessMaster
    {
        private IEnumerable<tLocFitnessCenter> _loc;

        public IEnumerable<tLocFitnessCenter> LoadAllData()
        {
            _loc = (from p in DbMaster.tLocFitnessCenters
                    select p);
            return _loc;
        }

        public Counter<tLocFitnessCenter> LoadDataGrid(IDataTablesRequest request)
        {
            //LoadAllData();
            //var locCounter = _loc.Count();

            //var filteredLoc = (from e in _loc
            //                   where
            //                       e.LAlamat.Contains(request.Search.Value)
            //                   select e);

            //var paged =
            //    filteredLoc.OrderBy(x => x.LocationID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tLocFitnessCenter>
            //{
            //    ListClass = paged.ToList(),
            //    Total = locCounter,
            //    TotalFilter = filteredLoc.Count()
            //};

            var loc = (from p in DbMaster.tLocFitnessCenters
                       select p);

            var locCounter = loc.Count();
            var filteredLoc = loc;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredLoc = filteredLoc.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tLocFitnessCenter>(listFilter).Compile();
                filteredLoc = filteredLoc.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tLocFitnessCenter> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredLoc.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredLoc.OrderBy(m => m.LocationID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tLocFitnessCenter>
            {
                ListClass = paged.ToList(),
                Total = locCounter,
                TotalFilter = filteredLoc.Count()
            };
        }

        public tLocFitnessCenter GetobjById(int id)
        {
            var loc = (from p in DbMaster.tLocFitnessCenters
                       where p.LocationID == id
                       select p).SingleOrDefault();
            return loc;
        }

        public void Insert(tLocFitnessCenter obj)
        {
            DbMaster.tLocFitnessCenters.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tLocFitnessCenter obj)
        {
            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<tLocFitnessCenter> LoadDataForDropdownlist()
        {
            IEnumerable<tLocFitnessCenter> dtds = (from p in DbMaster.tLocFitnessCenters
                                                   orderby p.LocationID
                                                   select p);
            return dtds;
        }

        public IEnumerable<tLocFitnessCenter> GetData(string searchTerm, int pageSize, int pageNum)
        {
            _loc = LoadDataForDropdownlist();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IEnumerable<tLocFitnessCenter> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return _loc
                .Where(
                    a =>
                        a.LAuth.Contains(searchTerm)
                );
        }
    }
}
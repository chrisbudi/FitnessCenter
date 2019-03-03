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
    public class DataServiceSalesAction : DbDataAccessMaster
    {
        public DataServiceSalesAction() { }

        public DataServiceSalesAction(IEnumerable<tSalesAction> salesActions)
        {
            _salesActions = salesActions;
        }

        private IEnumerable<tSalesAction> _salesActions { get; set; }

        public IEnumerable<tSalesAction> LoadAllData()
        {
            _salesActions = (from p in DbMaster.tSalesActions
                             select p);
            return _salesActions;
        }

        public Counter<tSalesAction> LoadData(IDataTablesRequest request)
        {
            //LoadAllData();
            //var actCounter = _salesActions.Count();

            //var filteredAct = (from e in _salesActions
            //                   where
            //                       e.ActionName.Contains(request.Search.Value)
            //                   select e);

            //var paged =
            //    filteredAct.OrderBy(x => x.SalesActionID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tSalesAction>
            //{
            //    ListClass = paged.ToList(),
            //    Total = actCounter,
            //    TotalFilter = filteredAct.Count()
            //};

            var sales = (from p in DbMaster.tSalesActions
                         select p);

            var actCounter = sales.Count();
            var filteredAct = sales;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredAct = filteredAct.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tSalesAction>(listFilter).Compile();
                filteredAct = filteredAct.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tSalesAction> paged;

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
                    filteredAct.OrderBy(m => m.SalesActionID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tSalesAction>
            {
                ListClass = paged.ToList(),
                Total = actCounter,
                TotalFilter = filteredAct.Count()
            };
        }

        public tSalesAction GetobjById(int Id)
        {
            var act = (from p in DbMaster.tSalesActions
                       where p.SalesActionID == Id
                       select p).SingleOrDefault();
            return act;
        }

        public void Insert(tSalesAction obj)
        {
            DbMaster.tSalesActions.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tSalesAction obj)
        {
            var updatedData = GetobjById(obj.SalesActionID);
            updatedData.ActionName = obj.ActionName;
            updatedData.Note = obj.Note;
            Save();
        }

        public int GetSalesActionId(EnumSalesAction sales)
        {
            var salesString = sales.ToString("F");
            return DbMaster.tSalesActions.Single(m => m.ActionName == salesString).SalesActionID;
        }
    }
}
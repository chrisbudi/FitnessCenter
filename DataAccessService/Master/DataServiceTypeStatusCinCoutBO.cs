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
    public class DataServiceTypeStatusCinCoutBO : DbDataAccessMaster
    {

        public IQueryable<tTypeStatusCinCout> LoadAllData()
        {
            return DbMaster.tTypeStatusCinCouts;
        }

        public tTypeStatusCinCout GetobjById(int Id)
        {
            var status = (from p in DbMaster.tTypeStatusCinCouts
                          where p.TypeStatusInOut == Id
                          select p).SingleOrDefault();
            return status;
        }

        public void Insert(tTypeStatusCinCout obj)
        {
            DbMaster.tTypeStatusCinCouts.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tTypeStatusCinCout obj)
        {
            tTypeStatusCinCout updatedData = GetobjById((int)obj.TypeStatusInOut);
            updatedData.NameStatusInOut = obj.NameStatusInOut;
            Save();
        }

        public Counter<tTypeStatusCinCout> LoadData(IDataTablesRequest request)
        {
            IQueryable<tTypeStatusCinCout> stat = (from p in DbMaster.tTypeStatusCinCouts
                                                   select p);

            int statCounter = stat.Count();

            //IQueryable<tTypeStatusCinCout> filtered = (from e in stat
            //                                           where
            //                                           e.NameStatusInOut.Contains(request.Search.Value)
            //                                           select e);

            //List<tTypeStatusCinCout> paged =
            //    filtered.OrderBy(x => x.TypeStatusInOut)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tTypeStatusCinCout> { ListClass = paged.ToList(), Total = statCounter, TotalFilter = filtered.Count() };

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
                var daleg = ExpressionBuilder.GetExpression<tTypeStatusCinCout>(listFilter).Compile();
                filtered = filtered.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tTypeStatusCinCout> paged;

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
                    filtered.OrderBy(m => m.TypeStatusInOut)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tTypeStatusCinCout>
            {
                ListClass = paged.ToList(),
                Total = statCounter,
                TotalFilter = filtered.Count()
            };
        }

        public int GetStatusId(string status)
        {
            var id = (from p in DbMaster.tTypeStatusCinCouts
                      where p.NameStatusInOut == status
                      select p.TypeStatusInOut).Single();
            return id;
        }
    }
}

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
    public class DataServiceRuangKelas : DbDataAccessMaster
    {

        private IQueryable<tRuangKela> _ruangKelas { get; set; }

        public DataServiceRuangKelas() { }

        public DataServiceRuangKelas(IQueryable<tRuangKela> ruangKelas)
        {
            _ruangKelas = ruangKelas;
        }

        public IEnumerable<tRuangKela> LoadAllData()
        {
            _ruangKelas = (from p in DbMaster.tRuangKelas
                           select p);
            return _ruangKelas;
        }

        public Counter<tRuangKela> LoadData(IDataTablesRequest request)
        {
            var ruangs = (from p in DbMaster.tRuangKelas
                          select p);

            var actCounter = ruangs.Count();
            var filteredRuang = ruangs;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredRuang = filteredRuang.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tRuangKela>(listFilter).Compile();
                filteredRuang = filteredRuang.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tRuangKela> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredRuang.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredRuang.OrderBy(m => m.NRuangKelas)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tRuangKela>
            {
                ListClass = paged.ToList(),
                Total = actCounter,
                TotalFilter = filteredRuang.Count()
            };
        }

        public tRuangKela GetobjById(int Id)
        {
            var act = (from p in DbMaster.tRuangKelas
                       where p.RuangKelasID == Id
                       select p).SingleOrDefault();
            return act;
        }

        public void Insert(tRuangKela obj)
        {
            DbMaster.tRuangKelas.Add(obj);
            Save();
        }

        public void Update(tRuangKela obj)
        {
            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }
    }
}
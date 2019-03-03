using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataObjects.Entities;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;

namespace DataAccessService.Instruktur
{
    public class DataServicePlanKelas : DbDataAccessInstructor
    {
        public IQueryable<trPlanKela> trPT { get; set; }

        public IQueryable<trPlanKela> LoadAllData()
        {
            return DbInstruktur.trPlanKelas;
        }

        public Counter<trPlanKela> LoadData(IDataTablesRequest request)
        {
            var stat = ((char)EnumMemberCheck.In).ToString();

            IQueryable<trPlanKela> per = (from q in DbInstruktur.trPlanKelas
                                              //where q.tMember.trAktifitasMembers.Any(m => m.Status == stat) &&
                                              //q.SisaJam != 0
                                          select q);

            var perCounter = per.Count();

            var filteredPer = per;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    var filter = new Filter()
                    {
                        Operation = EnumFilterOp.Contains,
                        PropertyName = column.Name,
                        Value = column.FilterSearch
                    };

                    listFilter.Add(filter);
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<trPlanKela>(listFilter).Compile();
                filteredPer = filteredPer.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trPlanKela> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredPer.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredPer.OrderBy(m => m.PlanKelasID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<trPlanKela>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }

        public trPlanKela GetobjById(int id)
        {
            var plan = (from p in DbInstruktur.trPlanKelas
                        where p.PlanKelasID == id
                        select p).First();
            return plan;
        }


        public void InsertPlan(trPlanKela obj)
        {
            DbInstruktur.Entry(obj).State = EntityState.Added;
            Save();
        }

        public void Delete(int Id)
        {
            var obj = GetobjById(Id);
            DbInstruktur.Entry(obj).State = EntityState.Deleted;
            Save();
        }

        public void UpdatePlan(trPlanKela obj)
        {
            DbInstruktur.Entry(obj).State = EntityState.Modified;
            Save();
        }

    }
}

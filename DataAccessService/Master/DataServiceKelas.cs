using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DataObjects.Shared;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Image;
using DataObjects.Entities;

namespace DataAccessService.Master
{
    public class DataServiceKelas : DbDataAccessMaster
    {

        public IQueryable<tKela> LoadAllData()
        {
            return DbMaster.tKelas;
        }

        public Counter<tKela> LoadData(IDataTablesRequest request)
        {
            //IQueryable<tKela> kls = (from p in DbMaster.tKelas
            //              select p);

            //int klsCounter = kls.Count();

            //IQueryable<tKela> filteredKls = (from e in kls
            //                      where
            //                      e.KNamaKelas.Contains(request.Search.Value)
            //                      select e);

            //List<tKela> paged = 
            //    filteredKls.OrderBy(x => x.KelasID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tKela> { ListClass = paged.ToList(), Total = klsCounter, TotalFilter = filteredKls.Count() };

            var kls = (from p in DbMaster.tKelas
                       select p);

            var klsCounter = kls.Count();
            var filteredKls = kls;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredKls = filteredKls.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tKela>(listFilter).Compile();
                filteredKls = filteredKls.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tKela> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredKls.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredKls.OrderBy(m => m.KelasID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tKela>
            {
                ListClass = paged.ToList(),
                Total = klsCounter,
                TotalFilter = filteredKls.Count()
            };
        }


        public tKela GetobjByID(int id)
        {
            var kls = (from p in DbMaster.tKelas
                       where p.KelasID == id
                       select p).SingleOrDefault();
            return kls;
        }

        public bool Insert(HttpPostedFileBase file, tKela kelas)
        {
            Image service = new Image();
            kelas.ImageKelas = service.ConvertToBytes(file);
            DbMaster.tKelas.Add(kelas);
            var i = Save();
            return i > 0;
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(HttpPostedFileBase file, tKela kelas)
        {
            Image service = new Image();
            kelas.ImageKelas = service.ConvertToBytes(file);
            DbMaster.Entry(kelas).State = EntityState.Modified;
            Save();
        }


    }
}

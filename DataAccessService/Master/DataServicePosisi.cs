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
    public class DataServicePosisi : DbDataAccessMaster
    {
        public IQueryable<tPosisi> Posisis { get; set; }

        public IQueryable<tPosisi> LoadAllData()
        {
            return DbMaster.tPosisis;
        }

        public Counter<tPosisi> LoadData(IDataTablesRequest request)
        {
            //IQueryable<tPosisi> posisi = (from p in DbMaster.tPosisis
            //                              select p);

            //int posisiCounter = posisi.Count();

            //IQueryable<tPosisi> filteredPosisi = (from e in posisi
            //                                      where
            //                                      e.PNamaPosisi.Contains(request.Search.Value)
            //                                      select e);

            //List<tPosisi> paged =
            //    filteredPosisi.OrderBy(x => x.PosisiID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tPosisi> { ListClass = paged.ToList(), Total = posisiCounter, TotalFilter = filteredPosisi.Count() };

            var posisi = (from p in DbMaster.tPosisis
                          select p);

            var posisiCounter = posisi.Count();
            var filteredPosisi = posisi;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredPosisi = filteredPosisi.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tPosisi>(listFilter).Compile();
                filteredPosisi = filteredPosisi.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tPosisi> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredPosisi.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredPosisi.OrderBy(m => m.PosisiID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tPosisi>
            {
                ListClass = paged.ToList(),
                Total = posisiCounter,
                TotalFilter = filteredPosisi.Count()
            };
        }


        public tPosisi GetobjById(int Id)
        {
            var posisi = (from p in DbMaster.tPosisis
                          where p.PosisiID == Id
                          select p).SingleOrDefault();
            return posisi;
        }


        public void Insert(tPosisi obj)
        {
            DbMaster.tPosisis.Add(obj);
            Save();
        }

        public int GetPosisiIdByName(string posisiName)
        {
            return DbMaster.tPosisis.Single(m => m.PNamaPosisi == posisiName).PosisiID;
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tPosisi obj)
        {
            tPosisi updatedData = GetobjById((int)obj.PosisiID);
            updatedData.PNamaPosisi = obj.PNamaPosisi;
            Save();
        }

        public List<tPosisi> LoadDataForDropdownlist()
        {
            List<tPosisi> pos = (from p in DbMaster.tPosisis
                                 orderby p.PNamaPosisi
                                 select p).ToList();
            return pos;
        }

        public List<tPosisi> GetData(string searchTerm, int pageSize, int pageNum)
        {
            Posisis = LoadDataForDropdownlist().AsQueryable();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<tPosisi> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return Posisis.Where(
                    a => a.PNamaPosisi.Like(searchTerm)
                );
        }
    }
}

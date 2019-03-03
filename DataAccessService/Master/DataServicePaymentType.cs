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
    public class DataServicePaymentType : DbDataAccessMaster
    {
        public IQueryable<tPaymentType> jnsKartu { get; set; }

        public IQueryable<tPaymentType> LoadAllData()
        {
            return (from p in DbMaster.tPaymentTypes
                    select p);
        }

        public Counter<tPaymentType> LoadData(IDataTablesRequest request)
        {
            jnsKartu = LoadAllData();

            int jnsCounter = jnsKartu.Count();

            //IQueryable<tPaymentType> filteredJenis = (from e in jnsKartu
            //                                          where
            //                                          e.NamaType.Contains(request.Search.Value)
            //                                          select e);

            //List<tPaymentType> paged =
            //    filteredJenis.OrderBy(x => x.PaymentTypeID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tPaymentType> { ListClass = paged.ToList(), Total = jnsCounter, TotalFilter = filteredJenis.Count() };

            var filteredJenis = jnsKartu;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredJenis = filteredJenis.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tPaymentType>(listFilter).Compile();
                filteredJenis = filteredJenis.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tPaymentType> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredJenis.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredJenis.OrderBy(m => m.PaymentTypeID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tPaymentType>
            {
                ListClass = paged.ToList(),
                Total = jnsCounter,
                TotalFilter = filteredJenis.Count()
            };
        }


        public tPaymentType GetobjById(int Id)
        {
            var jns = (from p in DbMaster.tPaymentTypes
                       where p.PaymentTypeID == Id
                       select p).SingleOrDefault();
            return jns;
        }


        public void Insert(tPaymentType obj)
        {
            DbMaster.tPaymentTypes.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tPaymentType obj)
        {
            tPaymentType updatedData = GetobjById((int)obj.PaymentTypeID);
            updatedData.NamaType = obj.NamaType;
            Save();
        }

        public List<tPaymentType> LoadDataForDropdownlist()
        {
            List<tPaymentType> jns = (from p in DbMaster.tPaymentTypes
                                      orderby p.NamaType
                                      select p).ToList();
            return jns;
        }

        public List<tPaymentType> GetData(string searchTerm, int pageSize, int pageNum)
        {
            jnsKartu = LoadDataForDropdownlist().AsQueryable();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<tPaymentType> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return jnsKartu.Where(
                    a => a.NamaType.Like(searchTerm)
                );
        }

        public IEnumerable<EnumPaymentType> LoadAllPaymentType()
        {
            return Enum.GetValues(typeof(EnumPaymentType)).Cast<EnumPaymentType>();
        }
    }
}

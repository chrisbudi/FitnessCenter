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
    public class DataServicePaketPT : DbDataAccessMaster
    {
        private IQueryable<tPaketPT> _paket;
        public IQueryable<tPaketPT> LoadAllData()
        {
            return DbMaster.tPaketPTs;
        }
        public Counter<tPaketPT> LoadData(IDataTablesRequest request)
        {
            var pkt = (from p in DbMaster.tPaketPTs
                       select p);

            var pktCounter = pkt.Count();

            //var filteredPkt = (from e in pkt
            //                   where
            //                       e.PPTNama.Contains(request.Search.Value)
            //                   select e);

            //var paged =
            //    filteredPkt.OrderBy(x => x.tPaketPTID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tPaketPT>
            //{
            //    ListClass = paged.ToList(),
            //    Total = pktCounter,
            //    TotalFilter = filteredPkt.Count()
            //};

            var filteredPkt = pkt;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredPkt = filteredPkt.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<tPaketPT>(listFilter).Compile();
                filteredPkt = filteredPkt.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tPaketPT> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredPkt.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredPkt.OrderBy(m => m.tPaketPTID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tPaketPT>
            {
                ListClass = paged.ToList(),
                Total = pktCounter,
                TotalFilter = filteredPkt.Count()
            };
        }

        public tPaketPT GetobjById(int Id)
        {
            var pkt = (from p in DbMaster.tPaketPTs
                       where p.tPaketPTID == Id
                       select p).SingleOrDefault();
            return pkt;
        }


        public void Insert(tPaketPT obj)
        {
            DbMaster.tPaketPTs.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tPaketPT obj)
        {
            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<tPaketPT> GetData(string searchTerm, int pageSize, int pageNum, bool forMembership)
        {
            _paket = LoadAllData().Where(m => m.PPTStatus && m.PPTMembership == forMembership);
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize);
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }


        public IEnumerable<tPaketPT> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return _paket.Where(a => a.PPTNama.Contains(searchTerm));
        }
    }
}
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
    public class DataServiceBank : DbDataAccessMaster
    {
        public IQueryable<tBank> banks { get; set; }

        public Counter<tBank> LoadData(IDataTablesRequest request)
        {
            //IQueryable<tBank> bank = (from p in DbMaster.tBanks
            //                              select p);

            //int bankCounter = bank.Count();

            //IQueryable<tBank> filteredBank = (from e in bank
            //                                      where
            //                                      e.NamaBank.Contains(request.Search.Value)
            //                                      select e);

            //List<tBank> paged =
            //    filteredBank.OrderBy(x => x.BankID)
            //        .Skip(request.Start)
            //        .Take(request.Length)
            //        .ToList();

            //return new Counter<tBank> { ListClass = paged.ToList(), Total = bankCounter, TotalFilter = filteredBank.Count() };

            var bank = (from p in DbMaster.tBanks
                        select p);

            var bankCounter = bank.Count();
            var filteredBank = bank;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredBank = filteredBank.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }


            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tBank> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredBank.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredBank.OrderBy(m => m.BankID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tBank>
            {
                ListClass = paged.ToList(),
                Total = bankCounter,
                TotalFilter = filteredBank.Count()
            };
        }


        public tBank GetobjById(int Id)
        {
            var bank = (from p in DbMaster.tBanks
                        where p.BankID == Id
                        select p).SingleOrDefault();
            return bank;
        }


        public void Insert(tBank obj)
        {
            DbMaster.tBanks.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tBank obj)
        {
            tBank updatedData = GetobjById((int)obj.BankID);
            updatedData.NamaBank = obj.NamaBank;
            Save();
        }

        public List<tBank> LoadDataForDropdownlist()
        {
            List<tBank> bank = (from p in DbMaster.tBanks
                                orderby p.NamaBank
                                select p).ToList();
            return bank;
        }

        public List<tBank> GetData(string searchTerm, int pageSize, int pageNum)
        {
            banks = LoadDataForDropdownlist().AsQueryable();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<tBank> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return banks.Where(
                    a => a.NamaBank.Like(searchTerm)
                );
        }

        public IEnumerable<tBank> LoadAllBank()
        {
            return DbMaster.tBanks;
        }
    }
}

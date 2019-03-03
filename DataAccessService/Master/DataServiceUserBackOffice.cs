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
    public class DataServiceUserBackOffice : DbDataAccessMaster
    {
        public IQueryable<tUserBackOffice> Users { get; set; }

        public void LocSave(string[] locBo, int boId)
        {
            var bo = locBo.ToList();
            var loc = from p in DbMaster.tLocFitnessCenters
                      where bo.Contains(p.LAuth)
                      select new
                      {
                          PersonBOID = boId,
                          LocationID = p.LocationID
                      };

            var locationList = loc.ToList().Select(item => new strLocBO()
            {
                PersonBOID = item.PersonBOID,
                LocationID = item.LocationID
            }).ToList();

            foreach (var strLocBo in locationList)
                DbMaster.strLocBOes.Add(strLocBo);

            Save();
        }

        public Counter<tUserBackOffice> LoadData(IDataTablesRequest request)
        {
            var user = (from p in DbMaster.tUserBackOffices
                        select p);

            var userCounter = user.Count();

            var filteredUser = user;
            List<Filter> listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                var filter = new Filter();
                var isFilter = false;
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    filteredUser = filteredUser.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");

                }
                if (column.FilterBool != null)
                {
                    isFilter = true;
                    filter = new Filter()
                    {
                        Operation = EnumFilterOp.Equals,
                        PropertyName = column.Name,
                        Value = column.FilterBool
                    };
                }

                if (isFilter)
                {
                    listFilter.Add(filter);
                }

            }

            if (listFilter.Any())
            {
                //var prop = typeof(tUserBackOffice).GetProperty("tPerson.PNama" );
                var daleg = ExpressionBuilder.GetExpression<tUserBackOffice>(listFilter).Compile();
                filteredUser = filteredUser.Where(daleg).AsQueryable();
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tUserBackOffice> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredUser.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredUser.OrderBy(m => m.BOIDNO)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<tUserBackOffice>
            {
                ListClass = paged.ToList(),
                Total = userCounter,
                TotalFilter = filteredUser.Count()
            };
        }


        public tUserBackOffice GetobjById(int Id)
        {
            var user = (from p in DbMaster.tUserBackOffices
                        where p.PersonBOID == Id
                        select p).SingleOrDefault();
            return user;
        }

        public void Insert(tUserBackOffice obj)
        {
            DbMaster.tUserBackOffices.Add(obj);
            Save();
        }

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tUserBackOffice obj)
        {

            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public tUserBackOffice GetLink(tUserBackOffice user)
        {
            user.tPosisi = new DataServicePosisi().GetobjById(user.PosisiID);
            return user;
        }

        public string GetLastNo()
        {
            var boid = (from p in DbMaster.tUserBackOffices.OrderByDescending(m => m.PersonBOID)
                        select p.BOIDNO).FirstOrDefault();
            var no = boid != null ? Int32.Parse(boid.Substring(6)) + 1 : 1;

            return DateTime.Now.ToString("yyyyMM") + no.ToString("000000");
        }

        public List<tUserBackOffice> LoadDataForDropdownlist()
        {
            List<tUserBackOffice> pos = (from p in DbMaster.tUserBackOffices
                                         orderby p.PosisiID
                                         select p).ToList();
            return pos;
        }

        public List<tUserBackOffice> GetData(string searchTerm, int pageSize, int pageNum, int posisiId)
        {
            Users = LoadDataForDropdownlist().AsQueryable().Where(m => m.StatusBOID && m.PosisiID == posisiId);
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<tUserBackOffice> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return Users.Where(
                    a => a.tPerson.PNama.Like(searchTerm)
                );
        }

        public IQueryable<tUserBackOffice> LoadAllData()
        {
            return DbMaster.tUserBackOffices;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;
using DataObjects.Entities;

namespace DataAccessService.PT
{
    public class DataServicePlanAktifitasPT : DbDataAccessPT
    {
        public IQueryable<trPlanAktifitasPT> trPT { get; set; }

        public IQueryable<trPlanAktifitasPT> LoadAllData()
        {
            return DbPersonalTrainer.trPlanAktifitasPTs;
        }

        public Counter<trPlanAktifitasPT> LoadData(IDataTablesRequest request)
        {
            var stat = ((char)EnumMemberCheck.In).ToString();

            IQueryable<trPlanAktifitasPT> per = (from q in DbPersonalTrainer.trPlanAktifitasPTs
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
                var daleg = ExpressionBuilder.GetExpression<trPlanAktifitasPT>(listFilter).Compile();
                filteredPer = filteredPer.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trPlanAktifitasPT> paged;

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
                    filteredPer.OrderBy(m => m.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<trPlanAktifitasPT>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }

        public int GetObjCountByMemberID(int memberID)
        {
            var tglMin = DateTime.MinValue.AddYears(1899);
            IQueryable<strKlaimPT> act = (from p in DbPersonalTrainer.strKlaimPTs
                                          where p.trPersonalTrainer.trMembership.tMember.MemberID == memberID &&
                                          (((DateTime)p.AkhirClaim).Year == tglMin.Year)
                                          select p);

            int actCounter = act.Count();
            return actCounter;
        }

        public trPersonalTrainer GetobjByMemberId(int memberID)
        {
            var stat = ((char)EnumMemberCheck.In).ToString();
            var act = (from q in DbPersonalTrainer.trPersonalTrainers
                       where q.trMembership.tMember.trAktifitasMembers.Any(m => m.Status == stat && m.tMember.MemberID == memberID)
                       select q).First();
            return act;
        }

        public trPlanAktifitasPT GetobjById(int id)
        {
            var plan = (from p in DbPersonalTrainer.trPlanAktifitasPTs
                        where p.PlanAktifitasPTID == id
                        select p).First();
            return plan;
        }


        public void InsertPlan(trPlanAktifitasPT obj)
        {
            DbPersonalTrainer.trPlanAktifitasPTs.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            var pt = DbPersonalTrainer.trPlanAktifitasPTs.Single(m => m.PlanAktifitasPTID == Id);
            DbPersonalTrainer.Entry(pt).State = EntityState.Deleted;
            Save();
        }

        public void UpdatePlan(trPlanAktifitasPT obj)
        {
            DbPersonalTrainer.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public Counter<trPlanAktifitasPT> LoadDataPeriod(IDataTablesRequest request)
        {
            var stat = ((char)EnumMemberCheck.In).ToString();
            var Period = DateTime.Now.ToString("yyyyMM");
            IQueryable<trPlanAktifitasPT> per = (from q in DbPersonalTrainer.trPlanAktifitasPTs
                                                 where q.period == Period
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
                var daleg = ExpressionBuilder.GetExpression<trPlanAktifitasPT>(listFilter).Compile();
                filteredPer = filteredPer.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trPlanAktifitasPT> paged;

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
                    filteredPer.OrderBy(m => m.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<trPlanAktifitasPT>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }
    }
}

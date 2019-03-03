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
    public class DataServiceClaimPT : DbDataAccessPT
    {
        public IQueryable<trPersonalTrainer> trPT { get; set; }

        public IQueryable<trPersonalTrainer> LoadAllData()
        {
            return DbPersonalTrainer.trPersonalTrainers;
        }

        public Counter<strKlaimPT> LoadData(IDataTablesRequest request)
        {

            IQueryable<strKlaimPT> per = (from q in DbPersonalTrainer.strKlaimPTs
                                          where q.trPersonalTrainer.trMembership.tMember.trAktifitasMembers.Any(m => m.Status == "In") &&
                                          q.trPersonalTrainer.SisaJam != 0
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
                var daleg = ExpressionBuilder.GetExpression<strKlaimPT>(listFilter).Compile();
                filteredPer = filteredPer.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<strKlaimPT> paged;

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
                    filteredPer.OrderBy(m => m.trPersonalTrainer.trMembership.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<strKlaimPT>
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
                                          where p.trPersonalTrainer.trMembership.tMember.MemberID == memberID //&&                             //                                          (p.AkhirClaim?.Year == tglMin.Year)
                                          select p);

            int actCounter = act.Count();
            return actCounter;
        }

        public trPersonalTrainer GetobjByMemberId(int memberID)
        {
            var act = (from q in DbPersonalTrainer.trPersonalTrainers
                           //where q.tMember.trAktifitasMembers.Any(m => m.Status == "In" && m.tMember.MemberID == memberID)
                       select q).First();
            return act;
        }

        public strKlaimPT GetobjById(int ptid)
        {
            //            var tglMin = DateTime.MinValue.AddYears(1899);
            var act = (from p in DbPersonalTrainer.strKlaimPTs
                       where p.trPersonalTrainerID == ptid
                       select p).First();
            return act;
        }


        public void InsertPT(strKlaimPT obj)
        {
            DbPersonalTrainer.strKlaimPTs.Add(obj);
            Save();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePT(strKlaimPT obj)
        {
            DbPersonalTrainer.Entry(obj).State = EntityState.Modified;
            Save();
        }

    }
}

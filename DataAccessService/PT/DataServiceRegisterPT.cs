using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;
using DataObjects.Entities;
using DataObjects.Shared;
using ViewModel.Membership.Registrasi;
using ViewModel.Trainer.Claim;
using ViewModel.Trainer.RegisterPT;

namespace DataAccessService.PT
{
    public class DataServicePersonalTrainer : DbDataAccessPT
    {
        public IQueryable<trPersonalTrainer> trPT { get; set; }

        public IQueryable<trPersonalTrainer> LoadAllData()
        {
            return DbPersonalTrainer.trPersonalTrainers;
        }

        public Counter<trMembership> LoadDataMember(IDataTablesRequest request)
        {
            IQueryable<trMembership> filteredMember = from m in DbPersonalTrainer.trMemberships
                                                      where m.MSTglSelesai > DateTime.Now 
                                                      select m;
            var perCounter = filteredMember.Count();
            var filteredPer = filteredMember;
            List<Filter> listFilter = new List<Filter>();

            filteredMember = request.Columns.
                Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(filteredMember, (current, column) =>
                current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredMember.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredMember.OrderBy(m => m.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }


        public Counter<trPersonalTrainer> LoadDataClosing(IDataTablesRequest request, EnumDaily enumpt)
        {
            IQueryable<trPersonalTrainer> filteredMember;

            if (enumpt == EnumDaily.PersonalTrainer)
            {
                filteredMember = (from a in DbPersonalTrainer.trPersonalTrainers
                                  where a.trMembership.PersonBOIDSales == 0
                                  select a);
            }
            else
            {
                filteredMember = (from a in DbPersonalTrainer.trPersonalTrainers
                                  where a.trMembership.PersonBOIDSales != 0
                                  select a);
            }

            var perCounter = filteredMember.Count();

            var filteredPer = filteredMember;
            List<Filter> listFilter = new List<Filter>();

            filteredMember = request.Columns.
                Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(filteredMember, (current, column) =>
                current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trPersonalTrainer> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredMember.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredMember.OrderBy(m => m.trMembership.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<trPersonalTrainer>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }

        public Counter<trPersonalTrainer> LoadDataRegistPersonalTrainer(IDataTablesRequest request, string username)
        {
            var stat = ((char)EnumMemberCheck.In).ToString();
            IQueryable<trPersonalTrainer> filteredMember = (from a in DbPersonalTrainer.trPersonalTrainers
                                                            select a);

            var perCounter = filteredMember.Count();

            var filteredPer = filteredMember;
            List<Filter> listFilter = new List<Filter>();

            filteredMember = request.Columns.
              Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(filteredMember, (current, column) =>
              current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trPersonalTrainer> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredMember.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredMember.OrderBy(m => m.trMembership.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<trPersonalTrainer>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }

        public IQueryable<trPersonalTrainer> GetObjRegisterIn()
        {
            var stat = ((char)EnumMemberCheck.In).ToString();
            IQueryable<trPersonalTrainer> per = (from q in DbPersonalTrainer.trPersonalTrainers
                                                 join a in DbPersonalTrainer.trAktifitasMembers
                                                 on q.trMembership.tMember.MemberID equals a.tMember.MemberID
                                                 where (a.Status == stat)
                                                 select q);

            return per;
        }

        public trPersonalTrainer GetobjByMemberId(string memberID)
        {
            var act = (from p in DbPersonalTrainer.trPersonalTrainers
                       where p.trMembership.tMember.MemberNO == memberID
                       select p).FirstOrDefault();
            return act;
        }

        public trPersonalTrainer GetobjById(int Id)
        {
            var act = (from p in DbPersonalTrainer.trPersonalTrainers
                       where p.trPersonalTrainerID == Id
                       select p).SingleOrDefault();
            return act;
        }

        public ViewModelPTCreate Create(int membershipid, int boidAdm, int lokasi)
        {
            var dMember = new DataServiceMembership();

            var createPt = new ViewModelPTCreate
            {
                PersonalTrainer = new trPersonalTrainer()
                {
                    trMembership = dMember.LoadAllData().Single(m => m.trMembershipID == membershipid)
                },
                MasterPayment = new ViewModelMasterPayment()
                {
                    MSTglMulai = DateTime.Now,
                }
            };


            return createPt;
        }

        public void InsertPT(trPersonalTrainer obj)
        {
            DbPersonalTrainer.trPersonalTrainers.Add(obj);
            Save();
        }

        public void InsertWithoutValidation(trPersonalTrainer obj)
        {
            DbPersonalTrainer.Configuration.ValidateOnSaveEnabled = false;
            DbPersonalTrainer.trPersonalTrainers.Add(obj);
            Save();
            DbPersonalTrainer.Configuration.ValidateOnSaveEnabled = true;
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePT(trPersonalTrainer obj)
        {
            DbPersonalTrainer.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<trPersonalTrainer> GetData(string searchTerm, int pageSize, int pageNum, out int memberList)
        {
            // ReSharper disable once ComplexConditionExpression
            var ptMember = LoadAllData()
                .Where(m => m.ParentID == null //&& m.PersonBOIDPT == ptid
                && (m.trMembership.tMember.tPerson.PNama.Contains(searchTerm) ||
                m.trMembership.tMember.tPerson.PAlamat.Contains(searchTerm) ||
                m.trPersonalTrainer_ParentID.trMembership.tMember.tPerson.PNama.Contains(searchTerm) ||
                m.trPersonalTrainer_ParentID.trMembership.tMember.tPerson.PAlamat.Contains(searchTerm)));
            memberList = ptMember.Count();

            return ptMember.OrderBy(m => m.trPersonalTrainerID)
                    .Skip(pageSize * (pageNum - 1))
                    .Take(pageSize);
        }


        public Counter<ClaimMemberPT> LoadDataClaim(IDataTablesRequest request, int ptId, int activeLocation)
        {
            IQueryable<ClaimMemberPT> filteredMember = (from p in DbPersonalTrainer.trPersonalTrainers//.Include(m => m.tMember).Include(m => m.tMember.tPerson)
                                                        join a in DbPersonalTrainer.trAktifitasMembers on p.trMembership.MemberID equals a.MemberID into pa
                                                        from a in pa.DefaultIfEmpty()
                                                            // on p.MemberID equals m.MemberID
                                                        where p.trPersonalTrainerID == ptId || p.ParentID == ptId &&
                                                        p.trMembership.LocationID == activeLocation //&& a.AMMulai == DateTime.Now
                                                        select new ClaimMemberPT()
                                                        {
                                                            PersonalTrainer = p,
                                                            AktifitasMember = a
                                                        }).Distinct();

            var perCounter = filteredMember.Count();

            var filteredPer = filteredMember;
            List<Filter> listFilter = new List<Filter>();

            filteredMember = request.Columns.
              Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(filteredMember, (current, column) =>
              current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<ClaimMemberPT> paged;

            if (ord.Orderable)
            {
                paged =
                    filteredMember.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filteredMember.OrderBy(m => m.PersonalTrainer.trMembership.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            return new Counter<ClaimMemberPT>
            {
                ListClass = paged.ToList(),
                Total = perCounter,
                TotalFilter = filteredPer.Count()
            };
        }

        public Counter<trPersonalTrainer> LoadDataClosing(IDataTablesRequest requestModel, EnumDaily e, EnumAcountingStatus accounting, out decimal? amount)
        {
            var dStatus = new DataServiceStatusMember();

            int memberId = 0;
            int calonMemberId = 0;
            memberId = dStatus.GetStatusId(EnumStatusMember.PersonalTrainer);
            string accountingstatus = accounting.ToString("F");
            switch (e)
            {
                case EnumDaily.PersonalTrainer:
                    trPT = LoadAllData().Where(m => m.trMembership.StatusMID == memberId );
                    break;
                case EnumDaily.Pos:
                    trPT = LoadAllData().Where(m => m.trMembership.StatusMID == memberId );
                    break;
            }
            trPT = accounting == EnumAcountingStatus.Null ? trPT.Where(m => m.trMembership.AccountingStatus == null) : trPT.Where(m => m.trMembership.AccountingStatus == accountingstatus);
            
            amount = trPT.Sum(m => m.trMembership.Total);
            //Untuk ordering

            var ord = requestModel.Columns.GetSortedColumns().Single();


            //            var colOrd = request.Columns.GetSortedColumns();

            IQueryable<trPersonalTrainer> paged;

            if (ord.IsOrdered)
            {
                paged =
                    trPT.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(requestModel.Start)
                        .Take(requestModel.Length);
            }
            else
            {
                paged =
                    trPT.OrderByDescending(m => m.trMembershipID)
                        .Skip(requestModel.Start)
                        .Take(requestModel.Length);
            }

            var memberCounter = trPT.Count();
            return new Counter<trPersonalTrainer>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = trPT.Count()
            };

        }
    }
}

using DataAccessService.Master;
using DataObjects.Entities;
using DataObjects.Shared;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using ViewModel.Membership.Registrasi;
using ViewModel.Report;

namespace DataAccessService.Registrasi
{
    public class DataServiceMembership : DbDataAccessRegistrasi
    {
        public IQueryable<trMembership> _mem { get; set; }

        public DataServiceMembership()
        {
        }

        public DataServiceMembership(IQueryable<trMembership> memberships)
        {
            _memberships = memberships;
        }

        private IQueryable<trMembership> _memberships { get; set; }

        public void MemberLocSave(strLocMember strLoc)
        {
            DbMembership.strLocMembers.Add(strLoc);
            Save();
        }

        public IQueryable<trMembership> LoadAllData()
        {
            return (from p in DbMembership.trMemberships
                    select p);
        }

        public IQueryable<LaporanSalesDetail> LoadDataReportHarian(string startDate, string endDate)
        {

            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);

            return (from m in DbMembership.trMemberships
                    join p in DbMembership.strPayments on m.trMembershipID equals p.strPaymentMember.trMembershipID
                    where m.MSTglMulai >= start && m.MSTglMulai <= end
                    select new LaporanSalesDetail()
                    {
                        admPrice = m.Admin ?? 0,
                        Amount = m.Total ?? 0,
                        Bank = p.trPaymentWith.tBank_BankID.NamaBank,
                        DOB = m.tMember.tPerson.PTglLahir ?? DateTime.MinValue,
                        InputAdmin = m.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                        InvoiceDate = m.MSTglMulai,
                        Phone = m.tMember.tPerson.PHP1,
                        Remark = p.trPaymentWith.TraceCode + " " + p.trPaymentWith.ApprCode,
                        Sales = m.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                        expired = m.MSTglSelesai,
                        memberid = m.tMember.MemberNO,
                        nama = m.tMember.tPerson.PNama,
                        payType = p.trPaymentWith.tPaymentType.NamaType,
                        Month = m.TotalMonth,
                        countNo = m.seq,
                        TransactionType = m.trPersonalTrainers.Any() ? "Personal Trainer" : "Membership",
                        TransactionDetailType = m.tMember.tMemberType.MemberType +
                                                (m.trPersonalTrainers.Any() ?
                                                " - " + m.trPersonalTrainers.FirstOrDefault(t => t.trMembershipID == m.trMembershipID).tPaketPT.PPTNama :
                                                "")

                    }).OrderBy(m => m.TransactionType).ThenBy(m => m.TransactionDetailType);
        }

        public IQueryable<LaporanSalesSummary> LoadDataReportSum(string startDate, string endDate)
        {
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);
            var sourceSales = (from m in DbMembership.trMemberships
                               where m.MSTglMulai >= start && m.MSTglMulai <= end
                               join p in DbMembership.strPayments on m.trMembershipID equals p.strPaymentMember.trMembershipID
                               group m by m.tMember.tMemberType into memberType
                               select new LaporanSalesSummary()
                               {
                                   countNo = 0,
                                   JenisTransaksi = memberType.Key.MemberType,
                                   TransaksiType = "Membership",
                                   TotalMember = memberType.Where(mt => !mt.trPersonalTrainers.Any()).Count(),
                                   totalAdmin = memberType.Where(mt => !mt.trPersonalTrainers.Any()).Sum(tr => tr.Admin) ?? 0,
                                   totalPaid = memberType.Where(mt => !mt.trPersonalTrainers.Any()).Sum(tr => tr.Total) ?? 0,
                               }).OrderBy(m => m.TransaksiType).ThenBy(m => m.JenisTransaksi);

            var sourcePT = (from m in DbMembership.trPersonalTrainers
                            where m.trMembership.MSTglMulai >= start && m.trMembership.MSTglMulai <= end
                            join p in DbMembership.strPayments on m.trMembershipID equals p.strPaymentMember.trMembershipID
                            group m by m.tPaketPT into PT
                            select new LaporanSalesSummary()
                            {
                                countNo = 0,
                                JenisTransaksi = PT.Key.PPTNama,
                                TransaksiType = "Personal Trainer",
                                TotalMember = PT.Count(),
                                totalAdmin = PT.Sum(tr => tr.trMembership.Admin) ?? 0,
                                totalPaid = PT.Sum(tr => tr.trMembership.Total) ?? 0
                            }).OrderBy(m => m.TransaksiType).ThenBy(m => m.JenisTransaksi);
            return sourceSales.Concat(sourcePT);
        }

        public Counter<trMembership> LoadDataDaily(IDataTablesRequest request)
        {
            _memberships = LoadAllData().Where(m => m.AccountingStatus == null);

            var filtered = _memberships;
            var listFilter = new List<Filter>();
            //            foreach (var column in request.Columns)
            //            {
            //                if (!string.IsNullOrEmpty(column.FilterSearch))
            //                {
            //                    var filter = new Filter
            //                    {
            //                        Operation = EnumFilterOp.Contains,
            //                        PropertyName = column.Name,
            //                        Value = column.FilterSearch
            //                    };
            //
            //                    listFilter.Add(filter);
            //                }
            //            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<trMembership>(listFilter).Compile();
                filtered = filtered.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged;

            if (ord.Orderable)
            {
                paged =
                    filtered.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filtered.OrderBy(m => m.StatusMID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            var memberCounter = _memberships.Count();
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filtered.Count()
            };

        }

        public Counter<trMembership> LoadData(IDataTablesRequest request, int user)
        {
            var dStatus = new DataServiceStatusMember();
            var inMemberId = dStatus.GetStatusId(EnumStatusMember.Membership);

            _memberships = LoadAllData();
            _memberships = _memberships.OrderByDescending(m => m.tMember.MemberID);


            var filtered = _memberships;
            _memberships = request.Columns.
                Where(column => !string.IsNullOrEmpty(column.FilterSearch)).Aggregate(_memberships, (current, column) =>
                current.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")"));

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged = ord.Orderable
                ? filtered.OrderByDescending(m => m.trMembershipID)//.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length)
                : filtered.OrderByDescending(m => m.trMembershipID)
                        .Skip(request.Start)
                        .Take(request.Length);
            var memberCounter = _memberships.Count();
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filtered.Count()
            };
        }

        public Counter<trMembership> LoadDataClosing(IDataTablesRequest request, EnumDaily e, EnumAcountingStatus enumstatus, out decimal? amount)
        {
            var dStatus = new DataServiceStatusMember();

            int memberId = 0;
            int calonMemberId = 0;

            string accountingstatus = enumstatus.ToString("F");
            switch (e)
            {
                case EnumDaily.Membership:
                    memberId = dStatus.GetStatusId(EnumStatusMember.Membership);
                    calonMemberId = dStatus.GetStatusId(EnumStatusMember.CalonMember);

                    _memberships = LoadAllData().Where(m => (m.StatusMID == memberId || m.MemberID == calonMemberId));


                    break;
                case EnumDaily.Freeze:
                    memberId = dStatus.GetStatusId(EnumStatusMember.Freeze);
                    _memberships = LoadAllData().Where(m => m.StatusMID == memberId && m.AccountingStatus == null);
                    break;
                case EnumDaily.Project:
                    memberId = dStatus.GetStatusId(EnumStatusMember.Membership);
                    _memberships = LoadAllData().Where(m => m.StatusMID == memberId && m.tMember.MemberNO.Contains("V") && m.AccountingStatus == null);
                    break;
                case EnumDaily.Transfer:
                    memberId = dStatus.GetStatusId(EnumStatusMember.Transfer);
                    _memberships = LoadAllData().Where(m => m.StatusMID == memberId && m.AccountingStatus == null);
                    break;

                case EnumDaily.PersonalTrainer:
                    _memberships = LoadAllData().Where(m => m.StatusMID == memberId && m.AccountingStatus == null);
                    break;
                case EnumDaily.Pos:
                    memberId = dStatus.GetStatusId(EnumStatusMember.Transfer);
                    _memberships = LoadAllData().Where(m => m.StatusMID == memberId && m.AccountingStatus == null);

                    break;
                case EnumDaily.Collection:
                    memberId = dStatus.GetStatusId(EnumStatusMember.Transfer);
                    _memberships = LoadAllData().Where(m => m.StatusMID == memberId && m.AccountingStatus == null);

                    break;
                default:
                    _memberships = LoadAllData();
                    break;
            }

            _memberships = enumstatus == EnumAcountingStatus.Null ? _memberships.Where(m => m.AccountingStatus == null) : _memberships.Where(m => m.AccountingStatus == accountingstatus);


            amount = _memberships.Sum(m => m.Total);
            //Untuk ordering

            var ord = request.Columns.GetSortedColumns().Single();


            //            var colOrd = request.Columns.GetSortedColumns();

            IQueryable<trMembership> paged;

            if (ord.IsOrdered)
            {
                paged =
                    _memberships.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    _memberships.OrderByDescending(m => m.trMembershipID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            var memberCounter = _memberships.Count();
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = _memberships.Count()
            };
        }

        public Counter<trMembership> PersonLoadData(IDataTablesRequest request)
        {
            var dStatus = new DataServiceStatusMember();
            _memberships = LoadAllData();
            _memberships =
                _memberships
                    .OrderByDescending(m => m.tMember.MemberID);

            var memberCounter = _memberships.Count();
            var filteredMember = _memberships.Where(m => m.tMember.tPerson.PNama.Contains(request.Search.Value) ||
                                                         m.tMember.MemberNO.Contains(request.Search.Value));

            var paged = filteredMember.OrderBy(x => x.tMember.MemberID).Skip(request.Start).Take(request.Length);

            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }

        public Counter<trMembership> CMLoad(IDataTablesRequest request)
        {
            var dStatus = new DataServiceStatusMember();
            var calonStatusId = dStatus.GetStatusId(EnumStatusMember.CalonMember);

            _memberships = LoadAllData();


            _memberships = _memberships.Where(m => m.tMember.MemberID == null &&
                                                   m.StatusMID == calonStatusId);

            var memberCounter = _memberships.Count();
            var filteredMember = _memberships.Where(m => m.tMember.tPerson.PNama.Contains(request.Search.Value))
                .Select(m => m);

            var paged = filteredMember.OrderBy(x => x.tMember.MemberID).Skip(request.Start).Take(request.Length);

            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }

        public trMembership GetobjByID(int Id)
        {
            var membership = (from p in DbMembership.trMemberships
                              where p.trMembershipID == Id
                              select p).SingleOrDefault();
            return membership;
        }

        public trMembership GetobjByPersonID(int Id)
        {
            var membership = (from p in DbMembership.trMemberships
                              where p.tMember.PersonID == Id
                              select p).FirstOrDefault();
            return membership;
        }

        public tMember GetMemberLogin(string memberno)
        {
            var member = (from p in DbMembership.trMemberships
                          where p.tMember.MemberNO == memberno
                          select p.tMember).SingleOrDefault();
            return member;
        }

        public void Insert(trMembership obj)
        {
            DbMembership.trMemberships.Add(obj);
            Save();
        }

        public void InsertWithoutValidation(trMembership obj)
        {
            DbMembership.Configuration.ValidateOnSaveEnabled = false;
            DbMembership.trMemberships.Add(obj);
            Save();

            DbMembership.Configuration.ValidateOnSaveEnabled = true;
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(trMembership obj)
        {
            DbMembership.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public ViewModelMembershipCreate Create(int locationId, int id = 0)
        {
            var masterPayment = new ViewModelMasterPayment
            {
                Cash = 0,
                Credit = 0,
                Debit = 0,
                Total = 0,
                TotalPayment = 0,
                MSTglMulai = DateTime.Now,
                RemainPayment = 0,
                TotalOverPayment = 0,
                CountMember = 0,
                MemberType = 0,
                TotalMonth = 0
            };

            var member = new trMembership();
            if (id != 0)
            {
                member = DbMembership.trMemberships.First(m => m.trMembershipID == id);
            }

            var createMembership = new ViewModelMembershipCreate
            {
                Membership = member,
                MasterPayment = masterPayment
            };

            if (member.ParentID != null)
            {
                var memberCount = member.trMembership_ParentID.trMemberships.Count() + 1;
                createMembership.PaymentMember = new strPaymentMember()
                {
                    MemberTypeID = member.tMember.MemberTypeID
                };
                createMembership.MemberCount = memberCount;
                createMembership.Membership.Subtotal = member.tMember.tMemberType.Biaya;
                createMembership.Membership.Total = member.tMember.tMemberType.Biaya;
            }

            return createMembership;
        }

        private string FormattingString(string auth)
        {
            var dn = auth + DateTime.Now.ToString("yyMM");

            return "";
        }

        public int GetLastMemberShipId()
        {
            return DbMembership.trMemberships.Select(m => m.trMembershipID).OrderByDescending(i => i).Take(1).Single();
        }

        public string GenerateMembershipAggrementId(int userLocation)
        {
            var dlocation = new DataServiceLocFitnessCenter();
            string userAuth = dlocation.GetobjById(userLocation).LAuth;
            var ag = DbMembership.trMemberships.Where(m => m.AgreementID.StartsWith(userAuth))
                .OrderByDescending(membership => membership.AgreementID).Take(1).SingleOrDefault();
            if (ag != null)
            {
                var id = int.Parse(ag.AgreementID.Replace(userAuth, "")) + 1;
                return userAuth + id.ToString("000000");
            }
            return userAuth + 1.ToString("000000");
        }

        public Counter<trMembership> LoadDataMember(IDataTablesRequest requestModel)
        {
            var dStatus = new DataServiceStatusMember();
            _memberships = LoadAllData();
            _memberships =
                _memberships.Where(
                    m => m.tMember.MemberID != null && m.StatusMID == dStatus.GetStatusId(EnumStatusMember.Membership));
            var memberCounter = _memberships.Count();
            var filteredMember = _memberships.Where(m => m.tMember.tPerson.PNama.Contains(requestModel.Search.Value));
            var paged = filteredMember.OrderBy(x => x.tMember.MemberID).Skip(requestModel.Start).Take(requestModel.Length);
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = filteredMember.Count()
            };
        }


        public List<trMembership> LoadDataForDropdownlist()
        {
            List<trMembership> pos = (from p in DbMembership.trMemberships
                                      orderby p.tMember.MemberID
                                      select p).ToList();
            return pos;
        }

        public List<trMembership> GetData(string searchTerm, int pageSize, int pageNum)
        {
            _mem = LoadDataForDropdownlist().AsQueryable();
            return GetDataQuery(searchTerm)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<trMembership> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return _mem.Where(
                    a => a.tMember.tPerson.PNama.Like(searchTerm) &&
                        a.tStatusMember.STKet.Equals(EnumMemberState.Member)
                );
        }

        public Counter<trMembership> LoadDataFreezeAndCancel(IDataTablesRequest request)
        {

            var dStatus = new DataServiceStatusMember();

            var freezeStatusId = dStatus.GetStatusId(EnumStatusMember.Freeze);
            var cancelStatusId = dStatus.GetStatusId(EnumStatusMember.Transfer);
            _memberships = LoadAllData();


            _memberships = _memberships.Where(m => m.ParentID != null && (
            m.tStatusMember.StatusMID == freezeStatusId ||
            m.tStatusMember.StatusMID == cancelStatusId));


            var filtered = _memberships;
            var listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (string.IsNullOrEmpty(column.FilterSearch)) continue;


                var filter = new Filter
                {
                    Operation = EnumFilterOp.Contains,
                    PropertyName = column.Name,
                    Value = column.FilterSearch
                };

                listFilter.Add(filter);
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<trMembership>(listFilter).Compile();
                filtered = filtered.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged;

            if (ord.Orderable)
            {
                paged =
                    filtered.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filtered.OrderByDescending(m => m.tMember.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            var memberCounter = _memberships.Count();
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = paged.Count()
            };
        }


        public Counter<trMembership> LoadDataRenewal(IDataTablesRequest request)
        {

            var dStatus = new DataServiceStatusMember();

            var statusMember = dStatus.GetStatusId(EnumStatusMember.Membership);
            _memberships = LoadAllData();
            _memberships = _memberships.Where(m => m.StatusMID == statusMember && SqlFunctions.DateAdd("month", -1, m.MSTglSelesai) <= DateTime.Now);


            var filtered = _memberships;
            var listFilter = new List<Filter>();
            foreach (var column in request.Columns)
            {
                if (string.IsNullOrEmpty(column.FilterSearch)) continue;


                var filter = new Filter
                {
                    Operation = EnumFilterOp.Contains,
                    PropertyName = column.Name,
                    Value = column.FilterSearch
                };
                listFilter.Add(filter);
            }

            if (listFilter.Any())
            {
                var daleg = ExpressionBuilder.GetExpression<trMembership>(listFilter).Compile();
                filtered = filtered.Where(daleg).AsQueryable();
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged;

            if (ord.Orderable)
            {
                paged =
                    filtered.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    filtered.OrderByDescending(m => m.trMembershipID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            var memberCounter = _memberships.Count();
            return new Counter<trMembership>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = paged.Count()
            };
        }

        public trMembership GetobjByMemberNo(string id)
        {

            return DbMembership.trMemberships.First(m => m.tMember.MemberNO == id);
        }


        public trMembership GetobjByActiveId(string checkinId)
        {
            return DbMembership.trMemberships.
                SingleOrDefault(m => m.ActivationCode == checkinId);
        }

        public tMemberType GetTypeMembership(int membershipId)
        {
            var type = (from p in DbMembership.strPaymentMembers
                        where p.trMembershipID == membershipId
                        select p).Distinct().First().tMemberType;
            return type;
        }

        public Counter<strPaymentMember> LoadDataCollection(IDataTablesRequest request)
        {
            var dStatus = new DataServiceStatusMember();

            var inMemberId = dStatus.GetStatusId(EnumStatusMember.Membership);

            var strpaymember = from p in DbMembership.strPaymentMembers
                .Where(m => m.trMembership_trMembershipID.StatusMID == inMemberId &&
                            !m.trMembership_trMembershipID.tMember.tMemberType.IsPaket)
                                   //&&
                                   //                            m.trMembership_trMembershipID.AccountingStatus == null
                                   //&& !m.statusBayar) //&& DbFunctions.AddDays(m.Tanggal, -5) < DateTime.Now)
                               select p;

            var ord = request.Columns.GetSortedColumns().Single();

            IQueryable<strPaymentMember> paged;

            if (ord.IsOrdered)
            {
                paged = strpaymember.OrderByDescending(m => m.trMembershipID)//OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged = strpaymember.OrderByDescending(m => m.trMembershipID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }

            var memberCounter = strpaymember.Count();
            return new Counter<strPaymentMember>
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = strpaymember.Count()
            };
        }

        public IEnumerable<trMembership> GetMember(string searchTerm, int pageSize, int pageNum, out int typeCount)
        {
            _memberships = LoadAllData();

            var membership = _memberships.Where(m => m.tMember.tPerson.PNama.Contains(searchTerm) ||
                                                     m.tMember.tPerson.PAlamat.Contains(searchTerm) ||
                                                     m.tMember.MemberNO.Contains(searchTerm));
            typeCount = membership.Count();

            return membership.OrderBy(m => m.trMembershipID)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize);
        }

        public Counter<trMembership> LoadDataAccounting(IDataTablesRequest requestModel, EnumDaily personalTrainer, EnumAcountingStatus closed, out decimal? amount)
        {
            throw new NotImplementedException();
        }
    }
}
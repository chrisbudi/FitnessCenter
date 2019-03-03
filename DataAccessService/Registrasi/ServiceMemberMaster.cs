using System;
using System.Linq;
using DataAccessResource.Master;
using DataAccessResource.Membership;
using DataObjects.Entities;
using DataObjects.Shared;
using Scheme.UOW;
using Services.Class;
using Services.DataTables;
using Services.Extensions;

namespace DataAccessService.Registrasi
{
    public interface IServiceMembership
    {
        Counter<trMembership> MembershipDTable(IDataTablesRequest request);
        void Insert(trMembership member);
        trMembership Get(int id);
        IQueryable<trMembership> Get();
        decimal FirstPayment(int? membershipId);
    }


    public class ServiceMembership : IServiceMembership
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceMembership(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public trMembership Get(string User)
        {
            var repo = _unitOfWork.GetDataRepository<MembershipRepo>();
            //var user = repo.Get().Where(m => m.tPerson.AspNetUser.UserName == User);
            return repo.Get().SingleOrDefault(m => m.tMember.tPerson.AspNetUser.UserName == User);
        }

        public IQueryable<trMembership> Get()
        {

            var repo = _unitOfWork.GetDataRepository<MembershipRepo>();
            return repo.Get();
        }

        public decimal FirstPayment(int? membershipId)
        {
            var repo = _unitOfWork.GetDataRepository<MembershipRepo>();
            //            return 0;
            var subtotal = repo.Get(membershipId ?? 0).Subtotal;

            return subtotal ?? 0;
        }

        public trMembership Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<MembershipRepo>();
            return repo.Get(id);
        }

        public void Insert(trMembership role)
        {
            var repo = _unitOfWork.GetDataRepository<MembershipRepo>();
            switch (role.trMembershipID)
            {
                case 0:
                    repo.Add(role);
                    break;
                default:
                    repo.Update(role);
                    break;
            }
        }

        Counter<trMembership> IServiceMembership.MembershipDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<MembershipRepo>();
            var roleEvent = repo.Get();
            var counter = roleEvent.Count();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    roleEvent = roleEvent.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            //Untuk ordering
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<trMembership> paged;

            if (ord.Orderable)
            {
                paged =
                    roleEvent.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    roleEvent.OrderBy(m => m.MemberID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<trMembership>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = roleEvent.Count()
            };
        }
    }
}

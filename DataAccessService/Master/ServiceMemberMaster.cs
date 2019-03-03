using Scheme.UOW;
using Services.Class;
using Services.DataTables;
using System.Linq;
using DataAccessResource.Master;
using DataObjects.Entities;
using DataObjects.Shared;
using Services.Extensions;

namespace DataAccessService.Master
{
    public interface IServiceMemberMaster
    {
        Counter<tMember> MemberDTable(IDataTablesRequest request);
        void Insert(tMember role);
        tMember Get(int id);
        tMember Get(string User);
        IQueryable<tMember> Get();
    }


    public class ServiceMemberMaster : IServiceMemberMaster
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceMemberMaster(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public tMember Get(string user)
        {
            var repo = _unitOfWork.GetDataRepository<MemberRepo>();
            return repo.Get().SingleOrDefault(m => m.MemberNO == user || m.trMemberships.Any(t => t.ActivationCode == user));
        }

        public IQueryable<tMember> Get()
        {

            var repo = _unitOfWork.GetDataRepository<MemberRepo>();
            return repo.Get();
        }

        public tMember Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<MemberRepo>();
            return repo.Get(id);
        }

        public void Insert(tMember role)
        {
            var repo = _unitOfWork.GetDataRepository<MemberRepo>();
            switch (role.MemberID)
            {
                case 0:
                    repo.Add(role);
                    break;
                default:
                    repo.Update(role);
                    break;
            }
        }

        Counter<tMember> IServiceMemberMaster.MemberDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<MemberRepo>();
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

            IQueryable<tMember> paged;

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


            return new Counter<tMember>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = roleEvent.Count()
            };
        }
    }
}

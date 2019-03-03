using DataAccessResource.Event;
using Scheme.UOW;
using Services.Class;
using Services.DataTables;
using System.Linq;
using DataObjects.Entities;
using DataObjects.Shared;
using Services.Extensions;

namespace DataAccessService.Event
{
    public interface IServiceRoleEvent
    {
        Counter<tRoleEvent> RoleEventDTable(IDataTablesRequest request);
        void Insert(tRoleEvent role);
        tRoleEvent Get(int id);
        IQueryable<tRoleEvent> Get();
    }


    public class ServiceRoleEvent : IServiceRoleEvent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRoleEvent(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<tRoleEvent> Get()
        {
            var repo = _unitOfWork.GetDataRepository<RoleEventRepo>();
            return repo.Get();
        }

        public tRoleEvent Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<RoleEventRepo>();
            return repo.Get(id);
        }

        public void Insert(tRoleEvent role)
        {
            var repo = _unitOfWork.GetDataRepository<RoleEventRepo>();
            switch (role.EvRoleId)
            {
                case 0:
                    repo.Add(role);
                    break;
                default:
                    repo.Update(role);
                    break;
            }
        }

        Counter<tRoleEvent> IServiceRoleEvent.RoleEventDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<RoleEventRepo>();
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

            IQueryable<tRoleEvent> paged;

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
                    roleEvent.OrderBy(m => m.EvRoleName)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<tRoleEvent>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = roleEvent.Count()
            };
        }
    }
}

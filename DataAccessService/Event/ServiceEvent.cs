using Services.Class;
using Services.DataTables;
using DataAccessResource.Event;
using Scheme.UOW;
using System.Linq;
using System.Collections.Generic;
using DataObjects.Entities;
using DataObjects.Shared;
using Services.Extensions;

namespace DataAccessService.Event
{
    public interface IServiceEvent
    {
        Counter<tEvent> EventDTable(IDataTablesRequest request);
        void Insert(tEvent e);
        tEvent Get(int id);
        List<tEvent> SelectData(string searchTerm, int pageSize, int pageNum, out int count);
    }

    public class ServiceEvent : IServiceEvent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceEvent(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public tEvent Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<EventRepo>();
            return repo.Get(id);
        }

        public List<tEvent> SelectData(string searchTerm, int pageSize, int pageNum, out int count)
        {
            var repo = _unitOfWork.GetDataRepository<EventRepo>();
            var events = repo.Get();
            count = events.Count();
            return events.Where(m => m.EvName.Contains(searchTerm))
                .OrderBy(m => m.EvName)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToList();
        }

        public void Insert(tEvent e)
        {
            var repo = _unitOfWork.GetDataRepository<EventRepo>();
            switch (e.EvId)
            {
                case 0:
                    repo.Add(e);
                    break;
                default:
                    repo.Update(e);
                    break;
            }
        }

        Counter<tEvent> IServiceEvent.EventDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<EventRepo>();
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

            IQueryable<tEvent> paged;

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
                    roleEvent.OrderBy(m => m.EvName)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<tEvent>
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = roleEvent.Count()
            };
        }
    }
}

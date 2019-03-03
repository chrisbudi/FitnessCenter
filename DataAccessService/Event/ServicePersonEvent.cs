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
    public interface IServicePersonEvent
    {
        Counter<trPersonEvent> PersonEventDTable(IDataTablesRequest request);
        void Insert(trPersonEvent person);
        trPersonEvent Get(int id);
    }

    public class ServicePersonEvent : IServicePersonEvent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePersonEvent(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public trPersonEvent Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<PersonEventRepo>();
            return repo.Get(id);
        }

        public void Insert(trPersonEvent person)
        {
            var repo = _unitOfWork.GetDataRepository<PersonEventRepo>();
            switch (person.PersonEventId)
            {
                case 0:
                    repo.Add(person);
                    break;
                default:
                    repo.Update(person);
                    break;
            }
        }

        public Counter<trPersonEvent> PersonEventDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<PersonEventRepo>();
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

            IQueryable<trPersonEvent> paged;

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
                    roleEvent.OrderBy(m => m.PersonId)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<trPersonEvent>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = roleEvent.Count()
            };
        }




    }
}

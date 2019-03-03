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
    public interface IServiceLocation
    {
        Counter<tLocFitnessCenter> LocDTable(IDataTablesRequest request);
        void Insert(tLocFitnessCenter loc);

        tLocFitnessCenter Get(int id);
        tLocFitnessCenter Get(string loc);
    }


    public class ServiceLocation : IServiceLocation
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceLocation(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Counter<tLocFitnessCenter> LocDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<LocationRepo>();
            var card = repo.Get();
            var counter = card.Count();
            foreach (var column in request.Columns)
            {
                if (!string.IsNullOrEmpty(column.FilterSearch))
                {
                    card = card.Where(column.Name + ".Contains(\"" + column.FilterSearch + "\")");
                }
            }

            //Untuk ordering 
            var ord = request.Columns.GetSortedColumns().First();

            IQueryable<tLocFitnessCenter> paged;

            if (ord.Orderable)
            {
                paged =
                    card.OrderUsingSortExpression(ord.Name + ' ' + ord.SortDirection)
                        .Skip(request.Start)
                        .Take(request.Length);
            }
            else
            {
                paged =
                    card.OrderBy(m => m.LocationID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<tLocFitnessCenter>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = card.Count()
            };
        }

        public void Insert(tLocFitnessCenter loc)
        {
            var repo = _unitOfWork.GetDataRepository<LocationRepo>();
            switch (loc.LocationID)
            {
                case 0:
                    repo.Add(loc);
                    break;
                default:
                    repo.Update(loc);
                    break;
            }
        }

        public tLocFitnessCenter Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<LocationRepo>();
            return repo.Get(id);
        }

        public tLocFitnessCenter Get(string loc)
        {
            var repo = _unitOfWork.GetDataRepository<LocationRepo>();
            return repo.Get().SingleOrDefault(statu => statu.LAuth == loc);
        }
    }
}

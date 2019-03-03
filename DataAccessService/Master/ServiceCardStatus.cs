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
    public interface IServiceCardStatus
    {
        Counter<tCardStatu> CardDTable(IDataTablesRequest request);
        void Insert(tCardStatu card);
        tCardStatu Get(int id);
        tCardStatu Get(string card);
        tCardStatu FinishedCard();
    }


    public class ServiceCardStatus : IServiceCardStatus
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCardStatus(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Counter<tCardStatu> CardDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<CardStatusRepo>();
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

            IQueryable<tCardStatu> paged;

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
                    card.OrderBy(m => m.CardStatusID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<tCardStatu>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = card.Count()
            };
        }

        public void Insert(tCardStatu card)
        {
            var repo = _unitOfWork.GetDataRepository<CardStatusRepo>();
            switch (card.CardStatusID)
            {
                case 0:
                    repo.Add(card);
                    break;
                default:
                    repo.Update(card);
                    break;
            }
        }

        public tCardStatu Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<CardStatusRepo>();
            return repo.Get(id);
        }

        public tCardStatu Get(string card)
        {
            var repo = _unitOfWork.GetDataRepository<CardStatusRepo>();
            return repo.Get().SingleOrDefault(statu => statu.CardStatus == card);
        }

        public tCardStatu FinishedCard()
        {
            var repo = _unitOfWork.GetDataRepository<CardStatusRepo>();
            return repo.Get().SingleOrDefault(m => m.FinalStatus);
        }
    }
}

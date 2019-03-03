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
    public interface IServiceBackOfficeMaster
    {
        Counter<tUserBackOffice> BackOfficeDTable(IDataTablesRequest request);
        void Insert(tUserBackOffice role);
        tUserBackOffice Get(int id);
        tUserBackOffice Get(string user);
    }


    public class ServiceBackOfficeMaster : IServiceBackOfficeMaster
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceBackOfficeMaster(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public tUserBackOffice Get(string User)
        {
            var repo = _unitOfWork.GetDataRepository<BackOfficeRepo>();
            return repo.Get().SingleOrDefault(m => m.tPerson.AspNetUser.UserName == User);
        }

        public tUserBackOffice Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<BackOfficeRepo>();
            return repo.Get(id);
        }

        public void Insert(tUserBackOffice role)
        {
            var repo = _unitOfWork.GetDataRepository<BackOfficeRepo>();
            switch (role.PersonBOID)
            {
                case 0:
                    repo.Add(role);
                    break;
                default:
                    repo.Update(role);
                    break;
            }
        }

        Counter<tUserBackOffice> IServiceBackOfficeMaster.BackOfficeDTable(IDataTablesRequest request)
        {
            var repo = _unitOfWork.GetDataRepository<BackOfficeRepo>();
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

            IQueryable<tUserBackOffice> paged;

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
                    roleEvent.OrderBy(m => m.PersonBOID)
                        .Skip(request.Start)
                        .Take(request.Length);
            }


            return new Counter<tUserBackOffice>()
            {
                ListClass = paged.ToList(),
                Total = counter,
                TotalFilter = roleEvent.Count()
            };
        }
    }
}

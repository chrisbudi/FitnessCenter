using System;
using System.Linq;
using DataAccessResource.PT;
using DataObjects.Entities;
using Scheme.UOW;

namespace DataAccessService.PT
{
    public class ServiceActionParam : IServiceActionParam
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceActionParam(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Insert(StrActionKlaimParam member)
        {
            var repo = _unitOfWork.GetDataRepository<ActionClaimParamRepo>();
            repo.Add(member);

        }

        public StrActionKlaimParam Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<ActionClaimParamRepo>();
            return repo.Get(id);
        }

        public IQueryable<StrActionKlaimParam> Get()
        {
            var repo = _unitOfWork.GetDataRepository<ActionClaimParamRepo>();
            return repo.Get();
        }
    }
}

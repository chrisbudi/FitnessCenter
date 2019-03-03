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
    public class ServicePaymentWith : IServicePaymentWith
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePaymentWith(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public void Insert(trPaymentWith member)
        {
            throw new NotImplementedException();
        }

        public trPaymentWith Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<trPaymentWith> Get()
        {
            throw new NotImplementedException();
        }
    }
}

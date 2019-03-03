using System.Linq;
using DataObjects.Entities;

namespace DataAccessService.Registrasi
{
    public interface IServicePaymentWith
    {

        void Insert(trPaymentWith member);
        trPaymentWith Get(int id);
        IQueryable<trPaymentWith> Get();
    }
}
using System;
using System.Linq;
using DataObjects.Entities;
using Services.Class;
using Services.DataTables;

namespace DataAccessService.Registrasi
{
    public class DataServiceAktivitasSales : DbDataAccessRegistrasi
    {
        public DataServiceAktivitasSales() { }

        public IQueryable<strAktivitasSale> _act = null;

        public IQueryable<strAktivitasSale> LoadAllData()
        {
            return from p in DbMembership.strAktivitasSales
                   select p;
        }

        public Counter<strAktivitasSale> ActLoad(MyCustomRequest request)
        {
            _act = LoadAllData();
            _act = _act.Where(m => m.trMembershipID == request.DecimalProp);
            var memberCounter = _act.Count();
            var paged = _act.OrderBy(x => x.AktivitasSalesID).Skip(request.Start).Take(request.Length);
            return new Counter<strAktivitasSale>()
            {
                ListClass = paged.ToList(),
                Total = memberCounter,
                TotalFilter = memberCounter
            };
        }

        public void Insert(strAktivitasSale sale)
        {
            DbMembership.strAktivitasSales.Add(sale);
            Save();
        }

        public strAktivitasSale Create(int id)
        {
            var sale = new strAktivitasSale()
            {
                trMembershipID = id,
                date = DateTime.Now
            };

            return sale;

        }

    }
}

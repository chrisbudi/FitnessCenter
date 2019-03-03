using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataObjects.Entities;
using Services.Class;
using Services.DataTables;
using Services.Helpers;

namespace DataAccessService.Master
{
    public class DataServicePerson : DbDataAccessMaster
    {
        private IEnumerable<tPerson> _persons;

        public IEnumerable<tPerson> LoadAllData()
        {
            _persons = (from p in DbMaster.tPersons
                        select p);
            return _persons;
        }

        public Counter<tPerson> LoadData(IDataTablesRequest request)
        {
            LoadAllData();
            var personCounter = _persons.Count();

            var filteredMember = _persons.Where(m => m.PNama.Contains(request.Search.Value))
                .Select(m => m);

            var paged = filteredMember.OrderBy(x => x.PersonID).Skip(request.Start).Take(request.Length);

            return new Counter<tPerson>() { ListClass = paged.ToList(), Total = personCounter, TotalFilter = filteredMember.Count() };
        }

        //public IEnumerable<EnumGender> LoadAllGenders()
        //{

        //}


        public tPerson GetobjById(int id)
        {
            var person = (from p in DbMaster.tPersons
                          where p.PersonID == id
                          select p).SingleOrDefault();
            return person;
        }
        public tPerson GetobjByUser(string user)
        {
            var person = (from p in DbMaster.tPersons
                          where p.AspNetUser.UserName == user
                          select p).SingleOrDefault();
            return person;
        }

        public void Insert(tPerson obj)
        {
            DbMaster.tPersons.Add(obj);
            Save();
        }

        public List<Select2StringResult> ListGenders()
        {
            List<Select2StringResult> list = new List<Select2StringResult>();
            foreach (var gender in Enum.GetValues(typeof(EnumGender)).Cast<EnumGender>().ToList())
            {
                var res = new Select2StringResult()
                {
                    id = gender.ToString().Substring(0, 1),
                    text = gender.ToString()
                };
                list.Add(res);
            }
            return list;
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(tPerson obj)
        {
            DbMaster.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public int GetLastPersonId()
        {
            var memberId = DbMaster.tPersons.Select(m => m.PersonID).OrderByDescending(m => m).Take(1);

            return memberId.Single();
        }

        public int GetLocation(tPerson person)
        {
            var locationsId = person.tMembers != null ? 0 : person.tUserBackOffice.strLocBOes.Single().LocationID;
            return locationsId;
        }


        public void InsertWithoutValidation(tPerson obj)
        {
            DbMaster.Configuration.ValidateOnSaveEnabled = false;
            DbMaster.tPersons.Add(obj);
            Save();
        }
    }
}

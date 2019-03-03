using System;
using System.Collections.Generic;
using System.Linq;
using IdentityModel.Model;
using Services.Class;
using Services.DataTables;
using Services.Extensions;

namespace DataAccessService.Identity
{
    public class GroupDataService : DbDataAccessIdentity
    {
        private IEnumerable<Select2StringResult> GroupList = null;

        public Counter<ApplicationGroup> LoadData(IDataTablesRequest request)
        {
            IQueryable<ApplicationGroup> forms = (from p in DbIdentity.ApplicationGroups
                                                  select p);

            int formsCounter = forms.Count();

            IQueryable<ApplicationGroup> filteredBank = (from e in forms
                                                         select e);

            string sortExpression;
            var ord = request.Columns.GetSortedColumns().First();
            switch (ord.Data)
            {
                case "2":
                    {
                        sortExpression = "Module ";
                        break;
                    }
                case "3":
                    {
                        sortExpression = "title ";
                        break;
                    }
                case "4":
                    {
                        sortExpression = "desciption ";
                        break;
                    }
                default:
                    {
                        sortExpression = "Module ";
                        break;
                    }
            }

            IQueryable<ApplicationGroup> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<ApplicationGroup>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public Counter<ApplicationGroup> CustomLoadDataa(IDataTablesRequest request)
        {
            IQueryable<ApplicationGroup> forms = (from p in DbIdentity.ApplicationGroups
                                                  select new ApplicationGroup());
            int formsCounter = forms.Count();

            IQueryable<ApplicationGroup> filteredBank = (from e in forms
                                                         select e);

            string sortExpression;
            var ord = request.Columns.GetSortedColumns().First();
            switch (ord.Data)
            {
                case "2":
                    {
                        sortExpression = "title ";
                        break;
                    }
                case "3":
                    {
                        sortExpression = "controller ";
                        break;
                    }
                case "4":
                    {
                        sortExpression = "action ";
                        break;
                    }
                default:
                    {
                        sortExpression = "title ";
                        break;
                    }
            }

            IQueryable<ApplicationGroup> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<ApplicationGroup>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public ApplicationGroup GetobjByID(string id)
        {
            ApplicationGroup bank = (from p in DbIdentity.Roles
                                     select new ApplicationGroup()
                ).SingleOrDefault(p => p.Id == id);
            return bank;
        }

        public void Insert(ApplicationGroup obj)
        {
            Save();
        }

        public void Delete(decimal id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationGroup obj)
        {
            ApplicationGroup updatedData = GetobjByID((string)obj.Id);
            Save();
        }

        public void LoadListGroup()
        {
            GroupList = (from p in DbIdentity.ApplicationGroups
                         select new Select2StringResult()
                         {
                             id = p.Id,
                             text = p.Name
                         }).ToList();
        }

        public List<Select2StringResult> ListGroupFormAuth(string id)
        {
            LoadListGroup();
            if (id == "")
                GroupList = GroupList
                    .Where(m => !DbIdentity.AspFormsAuthorizations
                        .Select(x => x.GroupId).Contains(m.id));
            else
                GroupList = GroupList.Where(m => m.id.Contains(id));

            return GroupList.ToList();
        }
    }
}
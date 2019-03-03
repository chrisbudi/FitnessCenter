using System;
using System.Linq;
using IdentityModel.Model;
using Services.Class;
using Services.DataTables;
using Services.Extensions;

namespace DataAccessService.Identity
{
    public class RolesDataService : DbDataAccessIdentity
    {

        public Counter<ApplicationRole> LoadData(IDataTablesRequest request)
        {
            IQueryable<ApplicationRole> forms = (from p in DbIdentity.Roles
                                                 select new ApplicationRole());

            int formsCounter = forms.Count();

            IQueryable<ApplicationRole> filteredBank = (from e in forms
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

            IQueryable<ApplicationRole> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<ApplicationRole>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public Counter<ApplicationRole> CustomLoadDataa(IDataTablesRequest request)
        {
            IQueryable<ApplicationRole> forms = (from p in DbIdentity.Roles
                                                 select new ApplicationRole());

            int formsCounter = forms.Count();

            IQueryable<ApplicationRole> filteredBank = (from e in forms
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

            IQueryable<ApplicationRole> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<ApplicationRole>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public ApplicationRole GetobjByID(string id)
        {
            ApplicationRole bank = (from p in DbIdentity.Roles
                                    select new ApplicationRole()
                {
                    Active = p.Active,
                    Description = p.Description,
                    Id = p.Id,
                    Name = p.Name
                }).SingleOrDefault(p => p.Id == id);
            return bank;
        }

        public void Insert(ApplicationRole obj)
        {
            Save();
        }

        public void Delete(decimal id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationRole obj)
        {
            ApplicationRole updatedData = GetobjByID((string)obj.Id);

            Save();
        }

        /// <summary>
        /// di gunakan untuk menyimpan data ketika create form
        /// </summary>
        /// <param name="roleName"></param>
        public void FormRoleSave(string controllerName)
        {
            foreach (char word in "CRUD")
            {
                var r = new ApplicationRole(controllerName + "_" + word);
                DbIdentity.Roles.Add(r);
            }

            Save();
        }

        public void FormRoleDelete(string controllerName)
        {
            var roles = DbIdentity.Roles.Where(m => m.Name.StartsWith(controllerName + "_"));
            foreach (var role in roles)
            {
                DbIdentity.Roles.Remove(role);
            }

            Save();
        }
    }
}
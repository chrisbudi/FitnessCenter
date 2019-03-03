using IdentityModel.Model;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ViewModel.Identity;

namespace DataAccessService.Identity
{
    public class FormRoleDataService : DbDataAccessIdentity
    {
        public Counter<ApplicationGroup> LoadData(IDataTablesRequest request)
        {
            IQueryable<ApplicationGroup> forms = (from p in DbIdentity.ApplicationGroups
                                                  select p);

            int formsCounter = forms.Count();

            IQueryable<ApplicationGroup> filteredGroup = (from e in forms
                                                          select e);

            string sortExpression;
            var ord = request.Columns.GetSortedColumns().First();
            switch (ord.Data)
            {

                default:
                    {
                        sortExpression = "Id ";
                        break;
                    }
            }

            IQueryable<ApplicationGroup> paged =
                filteredGroup.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<ApplicationGroup>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredGroup.Count()
            };
        }


        public Counter<AspFormAuthorization> CustomLoadDataa(IDataTablesRequest request)
        {
            IQueryable<AspFormAuthorization> forms = (from p in DbIdentity.AspFormsAuthorizations
                                                      select new AspFormAuthorization());

            int formsCounter = forms.Count();

            IQueryable<AspFormAuthorization> filteredBank = (from e in forms
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

            IQueryable<AspFormAuthorization> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ' ' + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<AspFormAuthorization>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public AspFormAuthorization GetobjByID(string id)
        {
            AspFormAuthorization bank = (from p in DbIdentity.AspFormsAuthorizations
                                         select new AspFormAuthorization()
                ).SingleOrDefault(p => p.GroupId == id);
            return bank;
        }

        //public void Insert(AspFormAuthorization obj)
        //{
        //    Save();
        //}
        public void Insert(IEnumerable<AspFormAuthorization> obj)
        {
            foreach (var form in obj)
            {
                DbIdentity.Entry(form).State = EntityState.Added;
            }
            Save();
        }

        public void Delete(decimal id)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<AspFormAuthorization> obj)
        {
            string groupID = obj.First().GroupId;
            var auth = from p in DbIdentity.AspFormsAuthorizations
                       where p.GroupId == groupID
                       select p;

            foreach (var au in auth.ToList())
            {
                DbIdentity.AspFormsAuthorizations.Remove(au);
            }

            foreach (var form in obj)
            {
                DbIdentity.Entry(form).State = EntityState.Added;
            }

            Save();
        }

        public List<Select2StringResult> GroupListSelect2()
        {
            var stdField = from p in DbIdentity.ApplicationGroups
                           where !(from x in DbIdentity.AspFormsAuthorizations
                                   select x.GroupId)
                               .Contains(p.Id)
                           select new Select2StringResult()
                           {
                               id = p.Id,
                               text = p.Name
                           };

            return stdField.ToList();
        }

        public FormRole Create()
        {
            var aspForm = new FormsDataService().LoadAllForm();
            var aspGroup = new GroupDataService().ListGroupFormAuth("");

            var listForm = new FormRole()
            {
                AspForms = aspForm,
                ApplicationGroups = aspGroup
            };
            return listForm;
        }

        public object Edit(string id)
        {
            var auths = from p in DbIdentity.AspFormsAuthorizations
                        where p.GroupId == id
                        select p;
            var aspForm = new FormsDataService().LoadAllForm();
            foreach (var form in aspForm)
            {
                foreach (var auth in auths)
                {
                    if (form.FormId == auth.FormID)
                    {
                        form.clickable = false;
                    }
                }
            }

            var aspGroup = new GroupDataService().ListGroupFormAuth(id);

            var listForm = new FormRole()
            {
                AspForms = aspForm,
                ApplicationGroups = aspGroup
            };
            return listForm;
        }
    }
}
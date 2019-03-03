using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
//using Fit.ViewStore.Model;
using IdentityModel.Model;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using ViewModel.Identity;

namespace DataAccessService.Identity
{
    public class FormAuthDataService : DbDataAccessIdentity
    {
        //fitnessDBEntities sp = new fitnessDBEntities();
        public Counter<AspFormAuthorization> LoadData(IDataTablesRequest request)
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

            IQueryable<AspFormAuthorization> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<AspFormAuthorization>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        //        public Counter<GET_FORMROLEAUTH_Result> CustomLoadDataCreate(MyCustomRequest requestModel)
        //        {
        //            string grup = "5b059b6c-be60-426c-921d-940fbd93bd7a";//requestModel.StringProp;
        //            IQueryable<GET_FORMROLEAUTH_Result> forms = (from p in sp.GET_FORMROLEAUTH(grup)
        //                                                         select p);
        //
        //            int formsCounter = forms.Count();
        //            IQueryable<GET_FORMROLEAUTH_Result> filteredBank = (from e in forms
        //                                                                select e);
        //
        //            string sortExpression;
        //            var ord = requestModel.Columns.GetSortedColumns().First();
        //            switch (ord.Data)
        //            {
        //
        //                default:
        //                    {
        //                        sortExpression = "Title ";
        //                        break;
        //                    }
        //            }
        //
        //            IQueryable<GET_FORMROLEAUTH_Result> paged =
        //                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
        //                    .Skip(requestModel.Start)
        //                    .Take(formsCounter);
        //
        //            return new Counter<GET_FORMROLEAUTH_Result>
        //            {
        //                ListClass = paged.ToList(),
        //                Total = formsCounter,
        //                TotalFilter = filteredBank.Count()
        //            };
        //        }

        public AspFormAuthorization GetobjByID(int id)
        {
            AspFormAuthorization bank = (from p in DbIdentity.AspFormsAuthorizations
                                         select new AspFormAuthorization()
                ).SingleOrDefault(p => p.FormID == id);
            return bank;
        }

        public void Insert(IEnumerable<ApplicationGroupRole> obj)
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

        public void Update(IEnumerable<ApplicationGroupRole> obj)
        {
            string groupId = obj.First().ApplicationGroupId;
            var group = (from p in DbIdentity.ApplicationGroupRoles
                         select p).Where(m => m.ApplicationGroupId == groupId);

            foreach (var app in group)
            {
                DbIdentity.ApplicationGroupRoles.Remove(app);
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

        public FormAuth Create()
        {
            var aspGroup = new GroupDataService().ListGroupFormAuth("");
            string id = "";
            if (aspGroup.Any())
                id = aspGroup.First().id;

            //var formRole = sp.GET_FORMROLEAUTH(id);
            var listForm = new FormAuth()
            {
                ApplicationGroups = aspGroup,
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
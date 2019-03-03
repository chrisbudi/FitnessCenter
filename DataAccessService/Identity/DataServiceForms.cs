using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IdentityModel.Model;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;
using ViewModel.Menu;

namespace DataAccessService.Identity
{
    public class FormsDataService : DbDataAccessIdentity
    {
        public Counter<AspForm> LoadData(IDataTablesRequest request)
        {
            IQueryable<AspForm> forms = (from p in DbIdentity.AspForms
                                         where p.parent_ID == null
                                         select p);

            int formsCounter = forms.Count();

            IQueryable<AspForm> filteredBank = (from e in forms
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

            IQueryable<AspForm> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(request.Start)
                    .Take(request.Length);

            return new Counter<AspForm>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public Counter<AspForm> CustomLoadDataa(MyCustomRequest customRequest)
        {
            decimal prop =
                (decimal.Parse(string.IsNullOrEmpty(customRequest.StringProp) ? "0" : customRequest.StringProp));
            IQueryable<AspForm> forms = (from p in DbIdentity.AspForms
                                         where (p.parent_ID == prop)
                                         select p);

            int formsCounter = forms.Count();

            IQueryable<AspForm> filteredBank = (from e in forms
                                                select e);

            string sortExpression;
            var ord = customRequest.Columns.GetSortedColumns().First();
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

            IQueryable<AspForm> paged =
                filteredBank.OrderUsingSortExpression(sortExpression + ord.SortDirection)
                    .Skip(customRequest.Start)
                    .Take(customRequest.Length);

            return new Counter<AspForm>
            {
                ListClass = paged.ToList(),
                Total = formsCounter,
                TotalFilter = filteredBank.Count()
            };
        }

        public AspForm GetobjByID(decimal id)
        {
            AspForm bank = (from p in DbIdentity.AspForms
                            where p.FormId == id
                            select p).SingleOrDefault();
            return bank;
        }

        public AspForm GetObjParentById(decimal id)
        {
            var parentForm = (from p in DbIdentity.AspForms
                              where p.FormId == id
                              select p).Single();
            var bank = new AspForm()
            {
                AspForm2 = parentForm,
                Module = parentForm.Module
            };
            return bank;
        }

        public void Insert(AspForm obj)
        {
            DbIdentity.AspForms.Add(obj);
            Save();
        }

        public void Delete(decimal id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update lebih cepat
        /// </summary>
        /// <param name="obj"></param>
        public void Update(AspForm obj)
        {
            //AspForm updatedData = GetobjByID((int)obj.FormId);
            DbIdentity.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public IEnumerable<AspForm> LoadAllForm()
        {
            var load = (from p in DbIdentity.AspForms
                        where p.parent_ID != null
                        select p);
            return load;
        }

        public IEnumerable<string> LoadAllModule()
        {
            return Enum.GetValues(typeof(EnumModuleForm)).Cast<EnumModuleForm>().Select(m => m.ToString()).ToList(); ;
        }


        public string GetActiveMenuByController(string controller)
        {
            var singleOrDefault = DbIdentity.AspForms.FirstOrDefault(m => m.controller == controller);
            return singleOrDefault != null ? singleOrDefault.Module : "";
        }

        public IEnumerable<Topbar> LoadModuleForms(string name, string menu)
        {
            var dIdentity = new DataServiceIdentity();
            return null;
        }

        //        /// <summary>
        //        /// Mengeload data sidebar berdasarkan controller
        //        /// </summary>
        //        /// <param name="name"></param>
        //        /// <param name="menu"></param>
        //        /// <param name="controller"></param>
        //        /// <param name="action"></param>
        //        /// <returns></returns>
        //        public IList<SideBar> LoadSideBarForms(string name, string menu, string controller, string action, string url)
        //        {
        //
        //            var dIdentity = new DataServiceIdentity();
        //            var user = dIdentity.GetUserByName(name);
        //            var forms = (from g in DbIdentity.ApplicationUserGroups
        //                         join fa in DbIdentity.AspFormsAuthorizations on g.ApplicationGroupId equals fa.GroupId
        //                         join child in DbIdentity.AspForms on fa.FormID equals child.FormId
        //                         join parent in DbIdentity.AspForms on child.parent_ID equals parent.FormId
        //                         join grandparent in DbIdentity.AspForms on parent.parent_ID equals grandparent.FormId
        //                         join parentAll in DbIdentity.AspForms on grandparent.FormId equals parentAll.parent_ID
        //                         where
        //                             g.ApplicationUserId == user.Id && parent.Module == menu && grandparent.parent_ID != null &&
        //                             child.controller == controller && child.action == action
        //                         select new SideBar
        //                         {
        //                             Icon = parentAll.iconclass,
        //                             Title = parentAll.title,
        //                             FormId = parentAll.FormId,
        //                             IsActive = parentAll.controller == parent.controller && parentAll.action == parent.action
        //                         }).Distinct().ToList();
        //
        //            foreach (var sideBar in forms)
        //            {
        //                var bar = sideBar;
        //                sideBar.SideBars =
        //                    (from g in DbIdentity.ApplicationUserGroups
        //                     join fa in DbIdentity.AspFormsAuthorizations on g.ApplicationGroupId equals fa.GroupId
        //                     join form in DbIdentity.AspForms on fa.FormID equals form.FormId
        //                     where g.ApplicationUserId == user.Id && form.parent_ID == bar.FormId
        //                     select new SidebarItem
        //                     {
        //                         Action = form.action,
        //                         Area = form.area,
        //                         IsActive = form.controller == controller && form.action == action,
        //                         Controller = form.controller,
        //                         IconClass = form.iconclass,
        //                         Title = form.title,
        //                         urlParameter = url
        //                     }).OrderBy(m => m.Title);
        //            }
        //            return forms.ToList();
        //        }



        public IList<SideNav> LoadSideBarForms(string name, string menu, string controller, string action, string url)
        {
            var dIdentity = new DataServiceIdentity();
            var user = dIdentity.GetUserByName(name);

            var forms = (from g in DbIdentity.ApplicationUserGroups
                         join fa in DbIdentity.AspFormsAuthorizations on g.ApplicationGroupId equals fa.GroupId
                         where g.ApplicationUserId == user.Id
                         select new SideNav
                         {
                             Title = fa.AspForm.MasterModule,
                             IsActive = fa.AspForm.MasterModule == menu
                         }).Distinct().ToList();

            //koding untuk mengisi nav file
            foreach (var subitem in forms)
            {
                subitem.Nav =
                    (from g in DbIdentity.ApplicationUserGroups
                     join fa in DbIdentity.AspFormsAuthorizations on g.ApplicationGroupId equals fa.GroupId
                     join form in DbIdentity.AspForms on fa.FormID equals form.FormId
                     where g.ApplicationUserId == user.Id && form.MasterModule == subitem.Title
                     select new SubNav
                     {
                         Icon = form.iconclass,
                         Module = form.Module,
                         MasterModule = form.MasterModule,

                         //                         IsActive = form.controller == controller
                     }).Distinct().OrderBy(m => m.Module).ToList();

                //koding untuk mengisi childnav navigasi
                foreach (var dtlItem in subitem.Nav)
                {
                    //                    dtlItem.IsActive = GetformIdFromControllerAndAction(controller, menu, dtlItem.MasterModule);
                    //= dtlItem.FormId == parentId;

                    dtlItem.NavDetail =
                        (from g in DbIdentity.ApplicationUserGroups
                         join fa in DbIdentity.AspFormsAuthorizations on g.ApplicationGroupId equals fa.GroupId
                         join form in DbIdentity.AspForms on fa.FormID equals form.FormId
                         where g.ApplicationUserId == user.Id && form.Module == dtlItem.Module && form.MasterModule == dtlItem.MasterModule
                         select new SidebarItem
                         {
                             Action = form.action,
                             Area = form.area,
                             IsActive = (form.controller == controller && (form.action == action || form.action == "Index")),
                             Controller = form.controller,
                             IconClass = form.iconclass,
                             Title = form.title,
                             urlParameter = ""
                         }).OrderBy(m => m.Title);

                    dtlItem.IsActive = dtlItem.NavDetail.Any(m => m.Controller.Equals(controller));
                }
            }


            return forms.ToList();

        }

        private bool GetformIdFromControllerAndAction(string controller, string module, string masterModule)
        {
            var form = DbIdentity.AspForms.Where(m => m.controller == controller && m.MasterModule == masterModule && m.Module == module);
            //            bool formact = form.Any(m => m.action == action);
            return form.Any();
        }


        //        /// <summary>
        //        /// mengeload data sidebar all
        //        /// </summary>
        //        /// <param name="name"></param>
        //        /// <param name="menu"></param>
        //        /// <returns></returns>
        //        public IList<SideBar> LoadSideBarForms(string name, string menu)
        //        {
        //
        //            var dIdentity = new DataServiceIdentity();
        //            var user = dIdentity.GetUserByName(name);
        //            var forms = (from g in DbIdentity.ApplicationUserGroups
        //                         join fa in DbIdentity.AspFormsAuthorizations on g.ApplicationGroupId equals fa.GroupId
        //                         join child in DbIdentity.AspForms on fa.FormID equals child.FormId
        //                         join parent in DbIdentity.AspForms on child.parent_ID equals parent.FormId
        //                         where g.ApplicationUserId == user.Id && parent.parent_ID == null
        //                         select new SideBar
        //                         {
        //                             Icon = fa.AspForm.AspForm2.iconclass,
        //                             Title = fa.AspForm.AspForm2.title,
        //                             FormId = parent.FormId
        //                         }).Distinct().ToList();
        //
        //            foreach (var sideBar in forms)
        //            {
        //                var bar = sideBar;
        //                sideBar.SideBars = DbIdentity.AspForms.Where(m => m.parent_ID == bar.FormId).
        //                    Select(m => new SidebarItem
        //                    {
        //                        Action = m.action,
        //                        Area = m.area,
        //                        IsActive = false,
        //                        Controller = m.controller,
        //                        IconClass = m.iconclass,
        //                        Title = m.title
        //                    }).OrderBy(m => m.Title);
        //            }
        //            return forms.ToList();
        //        }



    }
}
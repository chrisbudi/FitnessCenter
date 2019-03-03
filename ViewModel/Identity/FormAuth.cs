using System.Collections.Generic;
//using Fit.ViewStore.Model;
using Services.Class;

namespace ViewModel.Identity
{
    public class FormAuth
    {
        public List<Select2StringResult> ApplicationGroups { get; set; }
        public string GroupId { get; set; }
        public Operation[] Crud { get; set; }
        //public List<GET_FORMROLEAUTH_Result> FormAuthResults { get; set; }

        public class Operation
        {
            public string Id { get; set; }
            public string Selected { get; set; }
        }
    }

}
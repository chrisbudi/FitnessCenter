using System.Web.Mvc;
using IdentityModel.Principal;

namespace FitnessCenter.Views
{

    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new IdentityPrincipal User
        {
            get { return base.User as IdentityPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new IdentityPrincipal User
        {
            get { return base.User as IdentityPrincipal; }
        }
    }

}
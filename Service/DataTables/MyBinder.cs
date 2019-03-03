using System.Collections.Specialized;
using System.Web.Mvc;

namespace Services.DataTables
{
    public class MyBinder : DataTablesBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return Bind(controllerContext, bindingContext, typeof (MyCustomRequest));
        }

        protected override void MapAditionalProperties(IDataTablesRequest requestModel,
            NameValueCollection requestParameters)
        {
            var myModel = (MyCustomRequest) requestModel;
            myModel.StringProp = Get<string>(requestParameters, "idString");
            myModel.DecimalProp = Get<decimal>(requestParameters, "idDecimal");
        }
    }
}
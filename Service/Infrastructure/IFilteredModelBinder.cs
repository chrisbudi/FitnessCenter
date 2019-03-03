using System;
using System.Web.ModelBinding;

namespace Services.Infrastructure
{
    public interface IFilteredModelBinder : IModelBinder
    {
        bool IsMatch(Type modelType);

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Class;
using Services.DataTables;

namespace Fit.Interface
{
    public interface IMasterRepo<TModel> where TModel : class
    {
        // LOAD DATA DATATABLE
        Counter<TModel> LoadData(IDataTablesRequest request);
        // GET ID DATA FROM ACTION FORM
        TModel GetobjByID(int Id);
        // INSERT MODEL CLASS DATA
        void Insert(TModel obj);
        // DELETE MODEL CLASS DATA
        void Delete(int Id);
        // UPDATE MODEL CLASS DATA
        void Update(TModel obj);
        // EXEC METHOD VOID DATA
        void Save();
    }
}

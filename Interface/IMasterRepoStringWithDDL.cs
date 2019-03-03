using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Class;
using Services.DataTables;

namespace Fit.Interface
{
    public interface IMasterRepoStringWithDDL<TModel> where TModel : class
    {
        // LOAD DATA DATATABLE
        Counter<TModel> LoadData(IDataTablesRequest request);
        // LOAD DATA DROPDOWNLIST
        IEnumerable<TModel> LoadDataForDropdownlist();
        // GET ID DATA FROM ACTION FORM
        TModel GetobjByID(string Id);
        // INSERT MODEL CLASS DATA
        void Insert(TModel obj);
        // DELETE MODEL CLASS DATA
        void Delete(string Id);
        // UPDATE MODEL CLASS DATA
        void Update(TModel obj);
        // EXEC METHOD VOID DATA
        void Save();
        // GET DATA DROPDOWN LIST
        IEnumerable<TModel> GetData(string searchTerm, int pageSize, int pageNum);
        // COUNT DATA DROPDOWN LIST
        int GetDataCount(string searchTerm);
        // SET QUERY FROM GET DATA METHOD
        IEnumerable<TModel> GetDataQuery(string searchTerm);
    }
}

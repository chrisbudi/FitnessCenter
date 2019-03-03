using System.Collections.Generic;

namespace Services.Class
{
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2StringResult> Results { get; set; }
    }

    public class Select2StringResult
    {
        public string id { get; set; }
        public string text { get; set; }
        public string[] note { get; set; }
        public List<string> notelist { get; set; }
    }

    public class Select2DecimalResult
    {

        public decimal id { get; set; }
        public string text { get; set; }
    }
}
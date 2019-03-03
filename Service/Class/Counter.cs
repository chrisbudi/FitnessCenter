using System.Collections.Generic;

namespace Services.Class
{
    public class Counter<T>
    {
        public int Total { get; set; }
        public int TotalFilter { get; set; }
        public List<T> ListClass { get; set; }
    }
}

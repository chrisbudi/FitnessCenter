using Services.Helpers;

namespace Services.Class
{
    public class Filter
    {
        public string PropertyName { get; set; }
        public EnumFilterOp Operation { get; set; }
        public object Value { get; set; }
    }

}

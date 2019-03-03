namespace Services.DataTables
{
    public class MyCustomRequest : DefaultDataTablesRequest
    {
        public string StringProp { get; set; }
        public decimal DecimalProp { get; set; }
    }
}
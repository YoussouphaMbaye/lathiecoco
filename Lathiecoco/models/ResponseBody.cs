namespace Lathiecoco.models
{
    public class ResponseBody<T>
    {
        public T Body { get; set; }
        public string Msg { get; set; }
        public bool IsError { get; set; }=false;
        public int? CurrentPage { get; set; } = 1;
        public int? TotalPage { get; set; } = 1;
        public double? TotalCount { get; set; } = 0;
        public int? Code { get; set; } = 200;
    }
}

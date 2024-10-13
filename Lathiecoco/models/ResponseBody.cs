namespace Lathiecoco.models
{
    public class ResponseBody<T>
    {
        public T Body { get; set; }
        public string Msg { get; set; }
        public bool IsError { get; set; }=false;
        public int? CurrentPage { get; set; }
        public int? TotalPage { get; set; }
    }
}

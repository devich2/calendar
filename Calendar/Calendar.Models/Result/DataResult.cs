namespace Calendar.Models.Result
{
    public class DataResult<T> : Result
    {
        public T Data { get; set; }
    }
}
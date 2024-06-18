namespace Tusur.Practices.Application.Domain.Models.Result
{
    public class RequestResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class RequestResult<T> : RequestResult
        where T : class
    {
        public T? Value { get; set; }
    }
}

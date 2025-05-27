namespace Client.Models.Commons
{
    public sealed class BaseResult<T>
    {
        public BaseResult()
        {

        }
        public BaseResult(int statusCode, bool success, T? data = default, string? message = null)
        {
            StatusCode = statusCode;
            Success = success;
            Data = data;
            Message = message;
        }

        public int StatusCode { get; init; }
        public bool Success { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }
    }
}
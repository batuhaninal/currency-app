using Application.Abstractions.Commons.Results;

namespace Application.Models.DTOs.Commons.Results
{
    public class ResultDto : IBaseResult
    {
        public ResultDto(int statusCode, bool success, object? data, string? message)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
        }

        public ResultDto(int statusCode, bool success, object? data) : this(statusCode, success, data, null)
        {
            
        }

        public ResultDto(int statusCode, bool success) : this(statusCode, success, null, null)
        {
            
        }

        public int StatusCode { get; }

        public bool Success { get; }

        public string? Message { get; }

        public object? Data { get; }
    }
}

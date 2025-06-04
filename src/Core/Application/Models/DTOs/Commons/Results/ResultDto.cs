using Application.Abstractions.Commons.Results;

namespace Application.Models.DTOs.Commons.Results
{
    public class ResultDto : IBaseResult
    {
        public ResultDto(int statusCode, bool success, object? data, string? message, IDictionary<string, string[]>? validationErrors)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
            ValidationErrors = validationErrors;
        }
        
        public ResultDto(int statusCode, bool success, object? data, string? message) : this(statusCode, success, data, message, null)
        {

        }

        public ResultDto(int statusCode, bool success, object? data) : this(statusCode, success, data, null, null)
        {

        }

        public ResultDto(int statusCode, bool success) : this(statusCode, success, null, null, null)
        {
            
        }

        public int StatusCode { get; }

        public bool Success { get; }

        public string? Message { get; }

        public object? Data { get; }

        public IDictionary<string, string[]>? ValidationErrors { get; }
    }
}

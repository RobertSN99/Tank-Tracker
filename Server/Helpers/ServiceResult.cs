using Microsoft.AspNetCore.Identity;

namespace Server.Helpers
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public int? StatusCode { get; set; }

        public static ServiceResult<T> SuccessResult(T? data, string? message = null, int? statusCode = 200) => new()
        {
            Success = true,
            Data = data,
            Message = message,
            StatusCode = statusCode
        };

        public static ServiceResult<T> FailureResult(string message, IEnumerable<string>? errors = null, int? statusCode = 400) => new()
        {
            Success = false,
            Message = message,
            Errors = errors ?? [message],
            StatusCode = statusCode
        };
    }
}

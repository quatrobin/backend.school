namespace WebApplication3.Models.Common;

public interface IBaseResponse
{
    bool Success { get; set; }
    string Message { get; set; }
}

public interface IBaseResponse<T> : IBaseResponse
{
    T? Data { get; set; }
} 
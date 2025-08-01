namespace WebApplication3.Models.Common;

public class BaseResponse : IBaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    
    public static BaseResponse SuccessResponse(string message = "Операция выполнена успешно")
    {
        return new BaseResponse
        {
            Success = true,
            Message = message
        };
    }
    
    public static BaseResponse ErrorResponse(string message = "Произошла ошибка")
    {
        return new BaseResponse
        {
            Success = false,
            Message = message
        };
    }
}

public class BaseResponse<T> : IBaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    
    public static BaseResponse<T> SuccessResponse(T data, string message = "Операция выполнена успешно")
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
    
    public static BaseResponse<T> ErrorResponse(string message = "Произошла ошибка")
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
} 
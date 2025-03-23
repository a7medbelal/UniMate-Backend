
using Uni_Mate.Common.Data.Enums;

namespace Uni_Mate.Common.Views;

public record EndpointResponse<T>(T data, bool isSuccess, string message, ErrorCode errorCode)
{
    public static EndpointResponse<T> Success(T data, string message = "")
    {
        return new EndpointResponse<T>(data, true, message, ErrorCode.None);
    }

    public static EndpointResponse<T> Failure(ErrorCode errorCode, string message)
    {
        return new EndpointResponse<T>(default, false, message, errorCode);
    }
}
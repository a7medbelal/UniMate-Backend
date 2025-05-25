using Uni_Mate.Common.Data.Enums;

namespace Uni_Mate.Common.Views;

public record RequestResult<T>(T data, bool isSuccess, string message, ErrorCode errorCode)
{

    public static RequestResult<T> Success(T data, string message = "")
    {
        return new RequestResult<T>(data, true, message, ErrorCode.None);
    }


    public static RequestResult<T> Failure(ErrorCode errorCode, string message = "")
    {
        return new RequestResult<T>(default, false, message, errorCode);
    }
}
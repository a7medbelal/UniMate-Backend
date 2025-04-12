
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Middlewares;

public class GlobalErrorHandlerMiddleware
{
    RequestDelegate _nextAction;
    ILogger<GlobalErrorHandlerMiddleware> _logger;
    public GlobalErrorHandlerMiddleware(RequestDelegate nextAction, 
        ILogger<GlobalErrorHandlerMiddleware> logger)
    {
        _nextAction = nextAction;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _nextAction(context);
        }
        catch (Exception ex)
        {
            File.WriteAllText(@"F:\\log.txt", $"error{ex.Message}");
            var response = EndpointResponse<bool>.Failure(ErrorCode.UnknownError,"check the log file");
            context.Response.WriteAsJsonAsync(response);
        }

        //return SprintItem.CompletedTask;
    }
}
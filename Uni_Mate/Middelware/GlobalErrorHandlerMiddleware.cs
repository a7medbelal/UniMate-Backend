
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Middlewares;

public class GlobalErrorHandlerMiddleware
{
    RequestDelegate _nextAction;

    public GlobalErrorHandlerMiddleware(RequestDelegate nextAction)
    {
        _nextAction = nextAction;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _nextAction(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json"; // Return JSON response
                File.WriteAllText(@"F:\\log.txt", $"error{ex.Message}");

                var response = EndpointResponse<bool>.Failure(ErrorCode.ExpectionHappend, $"an error happen while processing the request");

                await context.Response.WriteAsJsonAsync(response);
            }
           // File.WriteAllText(@"/home/hossam/dotnet/errors.txt", $"error{ex.Message}");

        }

        //return SprintItem.CompletedTask;
    }
}
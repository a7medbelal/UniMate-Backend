
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;

namespace Uni_Mate.Middlewares;

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _nextAction;

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
                File.WriteAllText(@"C:\Users\Elnour Tech\source\repos\UniMate-Backend\notStarted.txt", $"error{ex.InnerException}");

                var response = EndpointResponse<bool>.Failure(ErrorCode.ExpectionHappend, $"an error happen while processing the request");

                await context.Response.WriteAsJsonAsync(response);
            }
            File.WriteAllText(@"C:\Users\Elnour Tech\source\repos\UniMate-Backend\started.txt", $"error{ex.Message}");

        }

        //return SprintItem.CompletedTask;
    }
}
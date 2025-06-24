
using Serilog;
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

            var message = ex.InnerException?.Message ?? ex.Message;

            var path = context.Request.Path;
            var method = context.Request.Method;
            var query = context.Request.QueryString.ToString();
            var headers = context.Request.Headers;
            var user = context.User.Identity?.Name ?? "Anonymous";

            Log.Error(ex,
                "Exception occurred:\n Method: {Method}\n Path: {Path}\n Query: {Query}\n User: {User}",
                method, path, query, user);


            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json"; // Return JSON response

                var response = EndpointResponse<bool>.Failure(ErrorCode.ExpectionHappend, $"an error happen while processing the request");

                await context.Response.WriteAsJsonAsync(response);
            }

        }

        //return SprintItem.CompletedTask;
    }
}
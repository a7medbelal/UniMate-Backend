using Microsoft.EntityFrameworkCore.Storage;
using Uni_Mate.Domain;

namespace TrelloCopy.Middlewares;

public class TransactionMiddleware
{
    RequestDelegate _nextAction;


    public TransactionMiddleware(RequestDelegate nextAction)
    {
        _nextAction = nextAction;

    }

    public async Task InvokeAsync(HttpContext context, Context _context)
    {
        IDbContextTransaction transaction = default;

        try
        {
            transaction = await _context.Database.BeginTransactionAsync();

            await _nextAction(context);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            //if (!context.Response.HasStarted)
            //{
            //    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //    context.Response.ContentType = "application/json"; // Return JSON response

            //    var errorResponse = new { message = "An error occurred while processing the request.", details = ex.Message };
            //    await context.Response.WriteAsJsonAsync(errorResponse);
            //}
            throw;
        }
    }
}
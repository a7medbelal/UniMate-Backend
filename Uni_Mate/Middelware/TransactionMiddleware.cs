using Microsoft.EntityFrameworkCore.Storage;
using Uni_Mate.Domain;

namespace Uni_Mate.Middlewares;

public class TransactionMiddleware
{
    private readonly RequestDelegate _nextAction;
    private readonly Context _context;

    public TransactionMiddleware(RequestDelegate nextAction, Context context)
    {
        _nextAction = nextAction;
        _context = context;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        IDbContextTransaction transaction = default;

        try
        {
            transaction = await _context.Database.BeginTransactionAsync();

            await _nextAction(context); // Process request (including SubmitPostCommand)

            await transaction.CommitAsync(); // Commit at the end
        }
        catch (Exception)
        {
            if (transaction != null)
             await transaction.RollbackAsync();

            throw;
        }
    }
}

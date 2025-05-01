using Microsoft.EntityFrameworkCore.Storage;
using Uni_Mate.Domain;

namespace TrelloCopy.Middlewares;

public class TransactionMiddleware
{
    RequestDelegate _nextAction;
    Context _context;
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
            transaction = _context.Database.BeginTransaction();

            await _nextAction(context);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction?.Rollback();

            throw;
        }
    }
}

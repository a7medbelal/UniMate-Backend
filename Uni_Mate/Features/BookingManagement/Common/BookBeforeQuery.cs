using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.Common;

public record BookBeforeQuery(string StudentId, int ApartmentId) : IRequest<RequestResult<bool>>;

public class BookBeforeQueryHandler : BaseRequestHandler<BookBeforeQuery, RequestResult<bool>, Booking>
{
    public BookBeforeQueryHandler(BaseRequestHandlerParameter<Booking> parameters) : base(parameters)
    {
    }
    public async override Task<RequestResult<bool>> Handle(BookBeforeQuery request, CancellationToken cancellationToken)
    {
        var booking = await _repository.AnyAsync(b => b.ApartmentId == request.ApartmentId && b.StudentId == request.StudentId);
        if (booking)
        {
            return RequestResult<bool>.Failure(ErrorCode.AlreadyExists, "User is already booked");
        }
        return RequestResult<bool>.Success(true, "User is booked in this apartment");
    }
}

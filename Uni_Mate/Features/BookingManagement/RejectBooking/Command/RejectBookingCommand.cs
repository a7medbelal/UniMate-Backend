using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.BookingManagement;
using Microsoft.EntityFrameworkCore;
namespace Uni_Mate.Features.BookingManagement.RejectBooking.Command;

    public record RejectBookingCommand(int BookingId) : IRequest<RequestResult<bool>>;

public class RejectBookingHandler : BaseRequestHandler<RejectBookingCommand, RequestResult<bool>, Booking>
{
    public RejectBookingHandler(BaseRequestHandlerParameter<Booking> parameters) : base(parameters)
    {
    }

    public override async Task<RequestResult<bool>> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
{
    if (request.BookingId <= 0)
    {
        return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "The Data Is Invalid");
    }

    var booking = await _repository.Get(x => x.Id == request.BookingId).FirstOrDefaultAsync();

    if (booking == null)
    {
        return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound,"The Booking Is Not Found");
    }

    if (booking.Status != Models.BookingManagement.BookingStatus.Pending)
    {
        return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, $"The Booking Is Already {booking.Status.ToString()}");
    }

    booking.Status = Models.BookingManagement.BookingStatus.Rejected;

    var updated = await _repository.SaveIncludeAsync(booking, nameof(booking.Status));
    if (!updated)
    {
        return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.SaveFailed, "The Saving Is Invalid .");
    }

    await _repository.SaveChangesAsync();

    return RequestResult<bool>.Success(true, "Reject Is Done .");
}

}

using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.BookingManagement;
using Uni_Mate.Models.BookingManagment;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;
public record AcceptBookingCommand(int BookingId) : IRequest<RequestResult<bool>>;

public class  AcceptBookingCommandHandler : BaseRequestHandler<AcceptBookingCommand, RequestResult<bool>, Booking>
{
    public AcceptBookingCommandHandler(BaseRequestHandlerParameter<Booking> parameter) : base(parameter)
    {
    }

    public override async Task<RequestResult<bool>> Handle(AcceptBookingCommand request, CancellationToken cancellationToken)
    {
        var Booking = await _repository.GetByIDAsync(request.BookingId);
        if (Booking == null)
        {
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "booking not found");
        }
        
        return RequestResult<bool>.Success(true);
    }

}

using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.Common;
using Uni_Mate.Features.Common;
using Uni_Mate.Models.BookingManagement;

namespace Uni_Mate.Features.BookingManagement.CancelBooking.Command
{
    public record CancelBookingCommand(int ApartmentId) : IRequest<RequestResult<bool>>;

    public class CancelBookingHandler : BaseRequestHandler<CancelBookingCommand, RequestResult<bool>, Booking>

    {

        public CancelBookingHandler(BaseRequestHandlerParameter<Booking> parameter) : base(parameter)
        {
        }
        public override async Task<RequestResult<bool>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            /* the steps to cancel the booking for any type
             * 1- check on the user .
             * 2- check on the apartment . 
             * 3- check on the if he can The abaility of book by booed before . 
             * 4- Just delete the parant and the child will be deleted if anyType Made A Hard Delete ^__^ haaa 
             */

            var userId = _userInfo.ID;
            if (string.IsNullOrEmpty(userId) || userId == "-1")
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            // check on the user 
            var userCheck = await _mediator.Send(new IsUserExistQuery(userId));
            if (!userCheck.isSuccess)
            {
                return RequestResult<bool>.Failure(userCheck.errorCode, userCheck.message);
            }


            if (request.ApartmentId <= 0)
            {
                return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found");
            }
            // check if the apartment exists
            var apartmentCheck = await _mediator.Send(new IsApartmentExistQuery(request.ApartmentId));
            if (!apartmentCheck.isSuccess)
            {
                return RequestResult<bool>.Failure(apartmentCheck.errorCode, apartmentCheck.message);
            }

            // check on the booking may he didn't book or there is error 
            var booking = _repository.Get(b => b.ApartmentId == request.ApartmentId && b.StudentId == userId).FirstOrDefault();
            if (booking == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "The student didn't make any book in this apartment");
            }

            await _repository.HardDelete(booking);
            return RequestResult<bool>.Success(true, "Booking cancelled successfully");
        }
    }
}

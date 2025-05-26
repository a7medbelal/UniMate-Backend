using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.CancelBooking.Command;

namespace Uni_Mate.Features.BookingManagement.CancelBooking
{
    [Authorize]
    public class CancelBookingEndpoint : BaseEndpoint<CancelBookingVM, bool>
    {
        public CancelBookingEndpoint(BaseEndpointParameters<CancelBookingVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> CancelBooking([FromBody] CancelBookingVM viewmodel, CancellationToken cancellationToken)
        {
            var validationResult = ValidateRequest(viewmodel);
            if (!validationResult.isSuccess)
            {
                return validationResult;
            }
            var result = await _mediator.Send(new CancelBookingCommand(viewmodel.apartmentId), cancellationToken);
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.data, "Booking cancelled successfully");
        }
    }
}

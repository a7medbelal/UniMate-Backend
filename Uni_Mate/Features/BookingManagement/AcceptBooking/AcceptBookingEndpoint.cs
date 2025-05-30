using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.AcceptBooking.Commands;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking
{
    [Authorize]
    public class AcceptBookingEndpoint: BaseEndpoint<AcceptBookingVM, bool>
    {
        public AcceptBookingEndpoint(BaseEndpointParameters<AcceptBookingVM> parameters):base(parameters)
        {

        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> AcceptBooking(AcceptBookingVM acceptBookingVM)
        {
            var valid = ValidateRequest(acceptBookingVM);
            if (!valid.isSuccess)
            {
                return EndpointResponse<bool>.Failure(valid.errorCode, valid.message);
            }

            var result = await _mediator.Send(new AcceptBookingCommand(acceptBookingVM.BookingId));

            if (result.isSuccess)
            {
                return EndpointResponse<bool>.Success(result.data, result.message);
            }

            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }
    }
}

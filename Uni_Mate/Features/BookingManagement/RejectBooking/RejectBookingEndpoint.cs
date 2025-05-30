using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.RejectBooking.Command;

namespace Uni_Mate.Features.BookingManagement.RejectBooking
{
    [Authorize]
    public class RejectBookingEndpoint : BaseEndpoint<RejectBookingVM,bool>
    {
        public RejectBookingEndpoint(BaseEndpointParameters<RejectBookingVM> parameters) : base(parameters)
        {
        }
        [HttpPost]
        public async Task<EndpointResponse<bool>> RejectBooking([FromBody] RejectBookingVM request)
        {
            var valid = ValidateRequest(request);
            if(!valid.isSuccess)
            {
                return EndpointResponse<bool>.Failure(valid.errorCode, valid.message);
            }

            var result = await _mediator.Send(new RejectBookingCommand(request.BookingId));
            if(!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }

            return EndpointResponse<bool>.Success(result.data,result.message);


        }

    }
}

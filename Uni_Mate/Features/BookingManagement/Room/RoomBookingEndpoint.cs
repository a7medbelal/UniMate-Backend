using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.Room.Command;

namespace Uni_Mate.Features.BookingManagement.Room
{
    [Authorize]
    public class RoomBookingEndpoint : BaseEndpoint<BookRoomVM, bool>
    {
        public RoomBookingEndpoint(BaseEndpointParameters<BookRoomVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> BookRoom([FromBody] BookRoomVM request)
        {
            var validationResult = ValidateRequest(request);
            if (!validationResult.isSuccess)
            {
                return EndpointResponse<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, validationResult.message);
            }
            var result = await _mediator.Send(new BookRoomCommand(request.apartmentId, request.roomId));
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.data);
        }
    }
}

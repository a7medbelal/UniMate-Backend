using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.BookingManagement.Apartments.Command;

namespace Uni_Mate.Features.BookingManagement.Apartments
{
    [Authorize]
    public class BookApartmentEndpoint : BaseEndpoint<BookApartmentVM, bool>
    {

        public BookApartmentEndpoint(BaseEndpointParameters<BookApartmentVM> parameters) : base(parameters)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> BookApartment([FromBody] BookApartmentVM request)
        {
            var validationResult = ValidateRequest(request);
            if (!validationResult.isSuccess)
            {
                return EndpointResponse<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, validationResult.message);
            }
            var result = await _mediator.Send(new BookApartmentCommand(request.ApartmentId));
            if (!result.isSuccess)
            {
                return EndpointResponse<bool>.Failure(result.errorCode, result.message);
            }
            return EndpointResponse<bool>.Success(result.data);
        }
    }
}

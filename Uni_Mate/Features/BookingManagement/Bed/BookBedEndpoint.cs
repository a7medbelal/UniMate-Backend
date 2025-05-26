using Uni_Mate.Common.Views;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Features.BookingManagement.BedViewModel;
using Uni_Mate.Features.BookingManagement.Beds.Commands;

public class BookBedEndpoint : BaseEndpoint<BookBedViewModel, bool>
{
    public BookBedEndpoint(BaseEndpointParameters<BookBedViewModel> parameters) : base(parameters)
    {
    }
    [HttpPost]
    public async Task<EndpointResponse<bool>> BookBed([FromBody] BookBedViewModel viewmodel,CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new BookBedCommand(viewmodel.RoomId, viewmodel.ApartmentId), cancellationToken);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(result.errorCode, result.message);
        }
        return EndpointResponse<bool>.Success(result.data, "Bed booked successfully");
    }
}
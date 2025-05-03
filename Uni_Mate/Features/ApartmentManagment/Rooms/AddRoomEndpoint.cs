using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.Rooms.Commands;

namespace Uni_Mate.Features.ApartmentManagment.Rooms;

public class AddRoomEndpoint : BaseEndpoint<AddRoomViewModel, bool>
{
    public AddRoomEndpoint(BaseEndpointParameters<AddRoomViewModel> parameters) : base(parameters)
    {
    }
    [HttpPost]
    public async Task<EndpointResponse<bool>> AddRoom([FromBody] AddRoomViewModel viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;
        var addRoomCommand = new AddRoomCommand(viewmodel.ApartmentId, viewmodel.Description, viewmodel.NumberOfBeds, viewmodel.Price, viewmodel.ImageUrl);
        var result = await _mediator.Send(addRoomCommand);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(ErrorCode.RoomCreationFailed, result.message);
        }
        return EndpointResponse<bool>.Success(result.isSuccess, "room added successfully");
    }
}
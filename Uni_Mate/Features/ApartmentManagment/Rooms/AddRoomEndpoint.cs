using ApartmentManagment.Features.ApartmentManagment.Rooms;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.Rooms.Commands;

namespace Uni_Mate.Features.ApartmentManagment.Rooms;

public class AddRoomEndpoint : BaseEndpoint<List<RoomBedViewModel>, bool>
{
    public AddRoomEndpoint(BaseEndpointParameters<List<RoomBedViewModel>> parameters) : base(parameters)
    {
    }
    [HttpPost]
    public async Task<EndpointResponse<bool>> AddRoom([FromBody] List<RoomBedViewModel> viewmodel)
    {
        var validationResult = ValidateRequest(viewmodel);
        if (!validationResult.isSuccess)
            return validationResult;
        var addRoomCommand = new AddRoomCommand(viewmodel);
        var result = await _mediator.Send(addRoomCommand);
        if (!result.isSuccess)
        {
            return EndpointResponse<bool>.Failure(ErrorCode.RoomCreationFailed, result.message);
        }
        return EndpointResponse<bool>.Success(result.isSuccess, "room added successfully");
    }
}
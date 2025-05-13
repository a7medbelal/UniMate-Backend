using ApartmentManagment.Features.ApartmentManagment.Rooms;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.Rooms.Commands;

public record AddRoomCommand(IList<RoomBedViewModel> RoomBedViewModels, int ApartmentId = 1) : IRequest<RequestResult<bool>>;

public class AddRoomCommandHandler : BaseRequestHandler<AddRoomCommand, RequestResult<bool>, Room>
{
    public AddRoomCommandHandler(BaseRequestHandlerParameter<Room> parameter) : base(parameter)
    {
        
    }

    public override async Task<RequestResult<bool>> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var rooms = request.RoomBedViewModels.Adapt<List<Room>>();
        foreach (var room in rooms)
        {
            room.ApartmentId = request.ApartmentId;
        }
        var result = _repository.AddRangeAsync(rooms);
        if(!result.IsCompletedSuccessfully)
        {
            return RequestResult<bool>.Failure(ErrorCode.RoomCreationFailed, "Failed to create room");
        }
        await _repository.SaveChangesAsync();
        return RequestResult<bool>.Success(true, "Room created successfully");
    }
}
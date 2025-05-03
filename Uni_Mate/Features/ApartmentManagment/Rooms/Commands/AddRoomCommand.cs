using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.Rooms.Commands;

public record AddRoomCommand(int ApartmentId, string? Description, int NoBeds, int Price, string? Image) : IRequest<RequestResult<bool>>;

public class AddRoomCommandHandler : BaseRequestHandler<AddRoomCommand, RequestResult<bool>, Room>
{
    public AddRoomCommandHandler(BaseRequestHandlerParameter<Room> parameter) : base(parameter)
    {
        
    }

    public override async Task<RequestResult<bool>> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            Description = request.Description,
            Price = request.Price,
            Image = request.Image,
            ApartmentId = request.ApartmentId,
        };
        var Beds = new List<Bed>();
        for (int i = 0; i < request.NoBeds; i++)
        {
            Beds.Add(new Bed
            {
                RoomId = room.Id,
                Price = request.Price,
                IsAvailable = true
            });
        }
        room.Beds = Beds;
        var result = _repository.AddAsync(room);
        if(!result.IsCompletedSuccessfully)
        {
            return RequestResult<bool>.Failure(ErrorCode.RoomCreationFailed, "Failed to create room");
        }
        await _repository.SaveChangesAsync();
        return RequestResult<bool>.Success(true, "Room created successfully");
    }
}
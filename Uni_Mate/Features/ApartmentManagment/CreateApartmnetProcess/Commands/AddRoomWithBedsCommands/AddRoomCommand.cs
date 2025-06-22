using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadImageCommand;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands;

public record AddRoomCommand(IList<RoomBedViewModel> RoomBedViewModels, int ApartmentId) : IRequest<RequestResult<RoomInfoDTO>>;

public class AddRoomCommandHandler : BaseRequestHandler<AddRoomCommand, RequestResult<RoomInfoDTO>, Room>
{
    public AddRoomCommandHandler(BaseRequestHandlerParameter<Room> parameter) : base(parameter)
    {
        
    }

    public override async Task<RequestResult<RoomInfoDTO>> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        //var rooms = request.RoomBedViewModels.Adapt<List<Room>>();
        //foreach (var room in rooms)
        //{
        //    room.ApartmentId = request.ApartmentId;
        //}

        var imageUploadTasks = request.RoomBedViewModels.Select(async r => await _mediator.Send(new UploadImageCommand(r.Image))).ToList();
        var imageUrls = await Task.WhenAll(imageUploadTasks);

        var rooms = request.RoomBedViewModels
        .Zip(imageUrls, (roomDto, imageUrl) => new Room
          {
              ApartmentId = request.ApartmentId,
              Description = roomDto.Description ,
              Capacity = roomDto.BedsNumber,
              Price = roomDto.Price, 
              IsAirConditioned = roomDto.HasAC,
              Image =  imageUrl.data,
              Beds = Enumerable.Range(0, roomDto.BedsNumber)
                         .Select(_ => new Bed {  Price= roomDto.Price })
                         .ToList()
          }).ToList();
     
           



        await _repository.AddRangeAsync(rooms);


        decimal priceOfTheApartment = rooms.Sum(r => r.Price);  
        var NumOfRooms = rooms.Count; 
        var NumOfBeds = rooms.Sum(r => r.Beds.Count);
        // Update the apartment's total price and number of rooms and beds
        var RoomInfo = new RoomInfoDTO
        {
            Price = priceOfTheApartment,
            NumberOfRooms = NumOfRooms,
            NumberOfBeds = NumOfBeds
        };  

        return RequestResult<RoomInfoDTO>.Success(RoomInfo, "Room created successfully");

        //foreach (var room in rooms)
        //{
        //    room.AddBeds(); // Add beds to each room based on its capacity
        //}

        // Handle photo upload for each room
        //foreach (var room in rooms)
        //{

        //    if (room.Image != null)
        //    {
        //        var uploadResult = await _mediator.Send(new UploadImageCommand());
        //        if (!uploadResult.isSuccess)
        //        {
        //            return RequestResult<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload room photo.");
        //        }
        //        room.Image = uploadResult.data;
        //    }
        //}
    }
}
public static class RoomExtensions
{
    public static void AddBeds(this Room room)
    {
        for (int i = 0; i < room.Capacity; i++)
        {
            room.Beds.Add(new Bed { RoomId = room.Id });
        }
    }
}
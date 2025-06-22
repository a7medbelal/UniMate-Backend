using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Domain.Repository;
using Microsoft.EntityFrameworkCore;


namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomDisplay.Queries
{
	public class UpdateApartmentRoomDisplayQuery : IRequest<RequestResult<UpdateApartmentRoomDisplayDTO>>
	{
		public int RoomId { get; set; }

		public UpdateApartmentRoomDisplayQuery(int roomId)
		{
			RoomId = roomId;
		}
	}

	public class UpdateApartmentRoomDisplayQueryHandler : IRequestHandler<UpdateApartmentRoomDisplayQuery, RequestResult<UpdateApartmentRoomDisplayDTO>>
	{
		private readonly IRepository<Room> _roomRepository;
		private readonly UserInfo _userInfo;

		public UpdateApartmentRoomDisplayQueryHandler(BaseRequestHandlerParameter<Room> parameters)
		{
			_roomRepository = parameters.Repository;
			_userInfo = parameters.UserInfo;
		}

		public async Task<RequestResult<UpdateApartmentRoomDisplayDTO>> Handle(UpdateApartmentRoomDisplayQuery request, CancellationToken cancellationToken)
		{
			var room = await _roomRepository.Get(c => c.Id == request.RoomId).Select(c => new {
				c.Description,
				c.Image,
				c.IsAirConditioned,
				c.Capacity,
				c.ApartmentId,
				c.Beds.FirstOrDefault().Price

			}).FirstOrDefaultAsync();

			if (room == null)
			{
				return RequestResult<UpdateApartmentRoomDisplayDTO>.Failure(ErrorCode.NotFound, "Room not found");
			}

			var OwnerID = _userInfo.ID;
			if (OwnerID == "-1")
			{
				return RequestResult<UpdateApartmentRoomDisplayDTO>.Failure(ErrorCode.Forbidden, "You are not authorized to access this room");
			}

			var dto = new UpdateApartmentRoomDisplayDTO
			{
				Description = room.Description,
				Image = room.Image,
				IsAirConditioned = room.IsAirConditioned,
				BedCount =room.Capacity,
				BedPrice = room.Price,
                Capacity = room.Capacity

			};

			return RequestResult<UpdateApartmentRoomDisplayDTO>.Success(dto);
		}
	}
}

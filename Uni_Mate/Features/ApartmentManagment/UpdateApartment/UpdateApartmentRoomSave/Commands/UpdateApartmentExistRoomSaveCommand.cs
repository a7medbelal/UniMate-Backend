using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.Common.DeleteImage.Commands;
using Uni_Mate.Features.Common.UploadImageCommand;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartmentRoomSave.Commands
{
	public record UpdateApartmentExistRoomSaveCommand(
		int RoomId,
		int ApartmentId,
		string RoomName,
		string Description,
		int NumOfBeds,
		decimal BedPrice,
		bool HasAC,
		IFormFile? RoomPhoto
		) : IRequest<RequestResult<bool>>;


	public class UpdateApartmentRoomSaveCommandHandler : BaseRequestHandler<UpdateApartmentExistRoomSaveCommand, RequestResult<bool>, Room>
	{
		private readonly IRepository<Apartment> _apartmentRepository;

		public UpdateApartmentRoomSaveCommandHandler(BaseRequestHandlerParameter<Room> parameters, IRepository<Apartment> apartmentRepository) : base(parameters)
		{
			_apartmentRepository = apartmentRepository;
		}

		public override async Task<RequestResult<bool>> Handle(UpdateApartmentExistRoomSaveCommand request, CancellationToken cancellationToken)
		{
			// Retrieve current owner ID from user info and check if authorized
			var ownerID = _userInfo.ID;

			if (string.IsNullOrEmpty(ownerID))
				return RequestResult<bool>.Failure(ErrorCode.OwnerNotAuthried, "Owner is not authorized.");

			// Fetch the existing room including related Apartment and Beds
			var room = await _repository.GetWithIncludeAsync(request.RoomId, "Apartment,Beds");

			if (room == null)
				return RequestResult<bool>.Failure(ErrorCode.RoomCreationFailed, "Room not found or not owned by you.");

			// Handle new room photo if provided (either update existing or add new)
			if (request.RoomPhoto != null)
			{
				if (!string.IsNullOrWhiteSpace(room.Image))
				{
					var deleteResult = await _mediator.Send(new DeleteImageCommand(room.Image));
					if (!deleteResult.isSuccess)
						return RequestResult<bool>.Failure(ErrorCode.InternalServerError, "Failed to delete the old image.");
				}

				var uploadResult = await _mediator.Send(new UploadImageCommand(request.RoomPhoto));
				if (!uploadResult.isSuccess)
					return RequestResult<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload the new image.");

				room.Image = uploadResult.data!;
			}

			// Adjusting the beds count and prices
			int currentBedsCount = room.Beds?.Count ?? 0;

			if (request.NumOfBeds > currentBedsCount)
			{
				for (int i = currentBedsCount; i < request.NumOfBeds; i++)
					room.Beds.Add(new Bed { Price = request.BedPrice });
			}
			else if (request.NumOfBeds < currentBedsCount)
			{
				int bedsToRemove = currentBedsCount - request.NumOfBeds;
				var bedsList = room.Beds!.ToList();

				for (int i = 0; i < bedsToRemove; i++)
					room.Beds.Remove(bedsList[bedsList.Count - 1 - i]);
			}
			else
			{
				foreach (var bed in room.Beds!)
					bed.Price = request.BedPrice;
			}

			// Calculate the total price based on the number of beds and their price
			decimal totalPrice = request.BedPrice * request.NumOfBeds;

			// Create a new Room object with updated values
			var roomToUpdate = new Room
			{
				Id = request.RoomId,
				ApartmentId = room.ApartmentId,
				Description = request.Description,
				IsAirConditioned = request.HasAC,
				Price = request.BedPrice * request.NumOfBeds,
				Beds = room.Beds,
				Image = room.Image
			};

			// Save the updated fields including beds and image
			await _repository.SaveIncludeAsync(roomToUpdate,
				nameof(roomToUpdate.Description),
				nameof(roomToUpdate.IsAirConditioned),
				nameof(roomToUpdate.Price),
				nameof(roomToUpdate.Beds),
				nameof(roomToUpdate.Image)
			);

			await _repository.SaveChangesAsync();

			// Update the total apartment price based on all its rooms
			if (room.Apartment != null)
			{
				var allRooms = room.Apartment.Rooms;
				if (allRooms != null && allRooms.Any())
				{
					room.Apartment.Price = allRooms.Sum(r => r.Price);

					await _apartmentRepository.SaveIncludeAsync(room.Apartment, nameof(room.Apartment.Price));
					await _apartmentRepository.SaveChangesAsync();
				}
			}

			return RequestResult<bool>.Success(true, "Room updated successfully.");
		}
	}
}
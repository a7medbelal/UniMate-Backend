using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.Common.UploadImageCommand;
using Uni_Mate.Models.ApartmentManagement;

public record UpdateApartmentAddRoomSaveCommand(

	// New Room info 
	int ApartmentId,
	string RoomName,
	string Description,
	int NumOfBeds,
	decimal BedPrice,
	bool HasAC,
	IFormFile? RoomPhoto
) : IRequest<RequestResult<bool>>;

public class UpdateApartmentAddRoomSaveCommandHandler : BaseRequestHandler<UpdateApartmentAddRoomSaveCommand, RequestResult<bool>, Apartment>
{
	public UpdateApartmentAddRoomSaveCommandHandler(BaseRequestHandlerParameter<Apartment> parameters)
		: base(parameters)
	{
	}

	public override async Task<RequestResult<bool>> Handle(UpdateApartmentAddRoomSaveCommand request, CancellationToken cancellationToken)
	{
		// Retrieve current owner ID from user info and check if authorized
		var ownerID = _userInfo.ID;
		if (string.IsNullOrEmpty(ownerID))
			return RequestResult<bool>.Failure(ErrorCode.OwnerNotAuthried, "Owner is not authorized.");

		// Retrieve the apartment with its associated rooms
		var apartment = await _repository.GetWithIncludeAsync(request.ApartmentId, "Rooms");
		if (apartment == null)
			return RequestResult<bool>.Failure(ErrorCode.ApartmentNotFound, "Apartment not found.");

		string? photoUrl = null;

		// Handle photo upload if a photo is provided
		if (request.RoomPhoto != null)
		{
			var uploadResult = await _mediator.Send(new UploadImageCommand(request.RoomPhoto));
			if (!uploadResult.isSuccess)
				return RequestResult<bool>.Failure(ErrorCode.UploadFailed, "Failed to upload room photo.");
			photoUrl = uploadResult.data;
		}

		// Calculate the total price based on the number of beds and their price
		decimal totalPrice = request.BedPrice * request.NumOfBeds;

		// Create the new room with its initial data
		var newRoom = new Room
		{
			ApartmentId = request.ApartmentId,
			Description = request.Description,
			IsAirConditioned = request.HasAC,
			Image = photoUrl,
			Beds = new List<Bed>()
		};

		// Add the required number of beds with the given price
		for (int i = 0; i < request.NumOfBeds; i++)
		{
			newRoom.Beds.Add(new Bed { Price = request.BedPrice });
		}

		// Set the room's total price
		newRoom.Price = totalPrice;

		// Add the new room to the apartment's room list
		apartment.Rooms.Add(newRoom);

		// Update the apartment's total price based on all its rooms
		apartment.Price = apartment.Rooms.Sum(r => r.Price);

		// Save the apartment including the newly added room and updated price
		await _repository.SaveIncludeAsync(apartment, nameof(apartment.Rooms), nameof(apartment.Price));
		await _repository.SaveChangesAsync();

		return RequestResult<bool>.Success(true, "Room added successfully.");
	}
}
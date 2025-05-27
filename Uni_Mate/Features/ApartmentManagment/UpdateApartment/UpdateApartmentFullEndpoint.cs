using Autofac.Features.Metadata;
using Microsoft.AspNetCore.Mvc;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoSave.Commands;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyImages.Commands;
using Uni_Mate.Features.ApartmentManagment.UpdateApartmentRoomSave.Commands;
namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment
{
	public class UpdateApartmentFullEndpoint : BaseEndpoint<UpdateApartmentFullViewModel, int>
	{
		public UpdateApartmentFullEndpoint(BaseEndpointParameters<UpdateApartmentFullViewModel> parameters) : base(parameters)
		{
		}

		[HttpPost]
		public async Task<EndpointResponse<int>> UpdateApartment([FromForm] UpdateApartmentFullViewModel request, CancellationToken cancellationToken)
		{
			var validationResult = _validator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
				return EndpointResponse<int>.Failure(ErrorCode.InvalidData, errors);
			}

			// Update Apartment Info
			var updateInfoResult = await _mediator.Send(new UpdateApartmentInfoSaveCommand(
				request.ApartmentId,
				request.Price,
				request.Location,
				request.Description,
				request.DescripeLocation,
				request.Floor,
				request.GenderAcceptance,
				request.DurationType
			));

			if (!updateInfoResult.isSuccess)
			{
				return EndpointResponse<int>.Failure(updateInfoResult.errorCode, updateInfoResult.message);
			}

			// Update or Add Each Room
			foreach (var room in request.Rooms)
			{
				if (room.RoomId.HasValue && room.RoomId.Value > 0)
				{
					// Existing room: update it
					var updateRoomResult = await _mediator.Send(new UpdateApartmentExistRoomSaveCommand(
						room.RoomId.Value,
						request.ApartmentId,
						room.RoomName,
						room.Description,
						room.BedCount,
						room.PricePerBed,
						room.HasAC,
						room.RoomPhoto
					));

					if (!updateRoomResult.isSuccess)
					{
						return EndpointResponse<int>.Failure(updateRoomResult.errorCode, $"Failed to update room '{room.RoomName}': {updateRoomResult.message}");
					}
				}
				else
				{
					// New room: add it
					var addRoomResult = await _mediator.Send(new UpdateApartmentAddRoomSaveCommand(
						request.ApartmentId,
						room.RoomName,
						room.Description,
						room.BedCount,
						room.PricePerBed,
						room.HasAC,
						room.RoomPhoto
					));

					if (!addRoomResult.isSuccess)
					{
						return EndpointResponse<int>.Failure(addRoomResult.errorCode, $"Failed to add room '{room.RoomName}': {addRoomResult.message}");
					}
				}
			}

			// Update Apartment Photos (add new or delete existing)
			var updateImages = new UpdateApartmentImagesCommand(
				request.ApartmentId,
				request.ApartmentDeleteImages,
				request.ApartmentNewImages
			);
			var updateImagesResult = await _mediator.Send(updateImages);

			if (!updateImagesResult.isSuccess)
			{
				return EndpointResponse<int>.Failure(ErrorCode.UpdateFailed, $"Failed to update apartment images: {updateImagesResult.message}");
			}

			return EndpointResponse<int>.Success(request.ApartmentId, "Apartment and rooms updated successfully.");
		}
	}
}
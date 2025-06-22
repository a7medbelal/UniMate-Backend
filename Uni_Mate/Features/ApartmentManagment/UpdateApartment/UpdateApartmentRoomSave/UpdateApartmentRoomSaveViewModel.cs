using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomSave
{
	public record UpdateApartmentRoomSaveViewModel(
	  // Because the room might not exist yet, we allow it to be nullable
    int RoomId,
    int ApartmentId,
	string Description,
	IFormFile? RoomPhoto,
	int BedCount,
	decimal PricePerBed,
	bool HasAC,
	int Capacity
);


	public class UpdateApartmentRoomSaveViewModelValidator : AbstractValidator<UpdateApartmentRoomSaveViewModel>
	{
		public UpdateApartmentRoomSaveViewModelValidator()
		{
	

			RuleFor(x => x.ApartmentId)
				.GreaterThan(0).WithMessage("Apartment ID is required.");

			RuleFor(x => x.BedCount)
				.GreaterThan(0).WithMessage("Bed count must be greater than zero. If you intend to delete the room, please remove it instead of setting the bed count to zero.");

			RuleFor(x => x.PricePerBed)
				.GreaterThan(0).WithMessage("Price per bed must be a positive value.");
		}
	}
}
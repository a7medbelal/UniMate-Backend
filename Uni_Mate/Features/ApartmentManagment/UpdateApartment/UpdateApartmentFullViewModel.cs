using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Models.GeneralEnum;
using System.Collections.Generic;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentRoomSave;
using FluentValidation;
using Uni_Mate.Features.Common.ApartmentManagement.UploadApartmentCommand;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentFacility;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment
{
	public record UpdateApartmentFullViewModel(
		int ApartmentId,
		string Price,
		string? Location,
		string Description,
		string? DescripeLocation,
		string Floor,
		Gender GenderAcceptance,
		ApartmentDurationType DurationType,
		int Capacity,
		List<UpdateApartmentRoomSaveViewModel> Rooms,
		List<UpdateApartmentFacilityViewModel> ApartmentFacilities,
		UploadImagesViewModel ApartmentNewImages,
		List<Image> ApartmentDeleteImages
	);

	public class UpdateApartmentFullViewModelValidator : AbstractValidator<UpdateApartmentFullViewModel>
	{
		public UpdateApartmentFullViewModelValidator()
		{
			RuleFor(x => x.ApartmentId)
				.GreaterThan(0).WithMessage("Apartment ID is required.");

			//RuleFor(x => x.Location)
			//	.NotEmpty().WithMessage("Location is required.");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.Matches(@"^[\p{L}\u0621-\u064A\d .,\-_\\r\\n]+$").WithMessage("Description must contain letters, digits, spaces, and allowed punctuation only.");

			RuleFor(x => x.DescripeLocation)
				.MaximumLength(300).WithMessage("Describe Location can't be longer than 300 characters.")
				.When(x => !string.IsNullOrEmpty(x.DescripeLocation))
				.Matches(@"^[\p{L}\u0621-\u064A\d .,\-_\\r\\n]+$").WithMessage("Location Description must contain letters, digits, spaces, and allowed punctuation (. , - _).");

			RuleFor(x => x.Floor)
				.NotEmpty().WithMessage("Floor is required.");

			RuleFor(x => x.GenderAcceptance)
				.IsInEnum().WithMessage("Invalid Gender.");

			RuleFor(x => x.DurationType)
				.IsInEnum().WithMessage("Invalid Duration.");

			RuleFor(x => x.Rooms)
				.NotNull().WithMessage("Rooms list cannot be null.")
				.Must(rooms => rooms.Any()).WithMessage("At least one room must be provided.");

			RuleForEach(x => x.Rooms)
				.SetValidator(new UpdateApartmentRoomSaveViewModelValidator());
		}
	}
}
using Uni_Mate.Models.GeneralEnum;
using Uni_Mate.Models.ApartmentManagement.Enum;
using Uni_Mate.Models.ApartmentManagement;
using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoSave
{
	public record UpdateApartmentInfoSaveViewModel(
		int ApartmentId,
		string Price,
		Location Location,
		string Description,
		string DescripeLocation,
		string Floor,
		Gender GenderAcceptance,
		ApartmentDurationType DurationType
	);

	public class UpdateApartmentInfoSaveViewModelValidator : AbstractValidator<UpdateApartmentInfoSaveViewModel>
	{
		// Validate required fields even if some are nullable in the model (e.g., Description, Floor) to ensure meaningful and complete apartment info, optional fields like DescripeLocation are skipped, and enums such as (Gender, DurationType) are ensured to have valid values
		public UpdateApartmentInfoSaveViewModelValidator()
		{
			RuleFor(x => x.ApartmentId)
				.GreaterThan(0).WithMessage("Apartment ID must be greater than zero.");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(500).WithMessage("Description can't be longer than 500 characters.")
				.Matches(@"^[\p{L}\d\s.,\-_]+$").WithMessage("Description must contain letters, digits, spaces, and allowed punctuation only.");


			RuleFor(x => x.DescripeLocation)
				.MaximumLength(300).WithMessage("Describe Location can't be longer than 300 characters.")
				.When(x => !string.IsNullOrEmpty(x.DescripeLocation))
				.Matches(@"^[\p{L}\d\s.,\-_]+$").WithMessage("Location Description must contain letters, digits, spaces, and allowed punctuation only.");

			RuleFor(x => x.Floor)
				.NotEmpty().WithMessage("Apartment Floor is required.")
				.MaximumLength(50).WithMessage("Floor info can't be longer than 50 characters.");

			RuleFor(x => x.GenderAcceptance)
				.IsInEnum().WithMessage("Invalid gender acceptance value.");

			RuleFor(x => x.DurationType)
				.IsInEnum().WithMessage("Invalid apartment duration type.");

			RuleFor(x => x.Price)
				.NotEmpty().WithMessage("Price is required.")
				.Must(price => decimal.TryParse(price, out var result) && result > 0).WithMessage("Price must be a valid positive number.");
		}
	}
}
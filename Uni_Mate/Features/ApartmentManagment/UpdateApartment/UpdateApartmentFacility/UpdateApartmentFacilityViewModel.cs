using FluentValidation;
using MediatR;
using Uni_Mate.Common.Views;
using Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentFacility
{
	public record UpdateApartmentFacilityViewModel(List<FacilityApartmentViewModel> Facilities, int ApartmentID) : IRequest<RequestResult<bool>>;
	public class UpdateApartmentFacilityViewModelValidator : AbstractValidator<UpdateApartmentFacilityViewModel>
	{
		public UpdateApartmentFacilityViewModelValidator()
		{
			RuleFor(x => x.ApartmentID)
			.GreaterThan(0).WithMessage("Apartment ID must be greater than zero.");

			RuleFor(x => x.Facilities)
			.NotNull().WithMessage("Facilities list cannot be null.")
			.Must(rooms => rooms.Any()).WithMessage("At least one facility must be provided.");
		}
	}
}

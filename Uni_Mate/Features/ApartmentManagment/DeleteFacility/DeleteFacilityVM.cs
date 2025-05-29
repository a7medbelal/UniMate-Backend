using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.DeleteFacility
{
    public record DeleteFacilityVM(int FacilityId);

    public class DeleteFacilityValidator : AbstractValidator<DeleteFacilityVM>
    {
        public DeleteFacilityValidator() {
            RuleFor(x => x.FacilityId)
                .GreaterThan(0).WithMessage("Invalid category ID.");
        }
    }
}

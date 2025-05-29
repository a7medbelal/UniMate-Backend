using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.AddFacility
{
    public record AddFacilityVM(string? Name, int CategoryId);

    public class AddFacilityVMValidator : AbstractValidator<AddFacilityVM>
    {
        public AddFacilityVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Facility name cannot be empty.")
                .MaximumLength(30).WithMessage("Facility name cannot exceed 30 characters.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Invalid category ID.");
        }
    }
}

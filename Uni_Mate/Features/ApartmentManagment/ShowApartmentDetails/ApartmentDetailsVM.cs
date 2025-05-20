using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.ShowApartmentDetails
{
    public record ApartmentDetailsVM(int id);

    public class ApartmentDetailsValidator : AbstractValidator<ApartmentDetailsVM>
    {
        public ApartmentDetailsValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Apartment ID is required.")
                .GreaterThan(0)
                .WithMessage("Apartmnet ID must be greater than 0.");    
        }
    }
}

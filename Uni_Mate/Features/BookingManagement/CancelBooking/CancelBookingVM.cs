using FluentValidation;

namespace Uni_Mate.Features.BookingManagement.CancelBooking
{
    public record CancelBookingVM(int apartmentId);

    public class CancelBookingValidator : AbstractValidator<CancelBookingVM>
    {
        public CancelBookingValidator()
        {
            RuleFor(x => x.apartmentId)
                .NotEmpty()
                .WithMessage("Apartment ID is required.")
                .GreaterThan(0)
                .WithMessage("Apartment ID must be greater than 0.");
        }
    }
}

using FluentValidation;

namespace Uni_Mate.Features.BookingManagement.RejectBooking
{
    public record RejectBookingVM(int BookingId);

    public class RejectBookingValidator : AbstractValidator<RejectBookingVM>
    {
        public RejectBookingValidator() {
            RuleFor(x => x.BookingId).NotEmpty()
                .WithMessage("Apartment Must Be Not Null .")
                .GreaterThan(0)
                .WithMessage("Apartment Must Be Greater Than Zero .");
        }
    }
}

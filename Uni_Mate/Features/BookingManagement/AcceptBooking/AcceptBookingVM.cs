using FluentValidation;

namespace Uni_Mate.Features.BookingManagement.AcceptBooking
{
    public record AcceptBookingVM(int BookingId);

    public class AcceptBookingValidator : AbstractValidator<AcceptBookingVM>
    {
        public AcceptBookingValidator() {
            RuleFor(x => x.BookingId).NotEmpty()
                .WithMessage("The Booking Approve Must Be Not Empty")
                .GreaterThan(0)
                .WithMessage("The Booking Aprove Must Be Greater Than Zero");
        }
    }
}

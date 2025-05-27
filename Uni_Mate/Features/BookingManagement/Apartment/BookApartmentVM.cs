using FluentValidation;

namespace Uni_Mate.Features.BookingManagement.Apartments
{
    public record BookApartmentVM(int ApartmentId);

    public class BookApartmentValidator : AbstractValidator<BookApartmentVM>
    {
        public BookApartmentValidator()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("Apartment ID is required")
                .GreaterThan(0).WithMessage("Apartment ID must be greater than 0");
        }
    }
}

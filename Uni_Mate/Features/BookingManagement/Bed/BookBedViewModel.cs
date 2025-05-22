using FluentValidation;

namespace Uni_Mate.Features.BookingManagement.BedViewModel;
public record BookBedViewModel(int BedId, int ApartmentId, int RoomId);

public class BookBedViewModelValidation : AbstractValidator<BookBedViewModel>
{
    public BookBedViewModelValidation()
    {
        RuleFor(x => x.BedId).NotEmpty().WithMessage("Bed ID is required.");
        RuleFor(x => x.ApartmentId).NotEmpty().WithMessage("Apartment ID is required.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Room ID is required.");
    }
}

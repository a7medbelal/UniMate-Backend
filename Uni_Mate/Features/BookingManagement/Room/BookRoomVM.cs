using FluentValidation;

namespace Uni_Mate.Features.BookingManagement.Room
{
    public record   BookRoomVM(int apartmentId, int roomId);

    public class BookRoomVMValidator : AbstractValidator<BookRoomVM>
    {
        public BookRoomVMValidator()
        {
            RuleFor(x => x.apartmentId).NotEmpty().WithMessage("Apartment ID is required.")
                .GreaterThan(0).WithMessage("Apartment ID must be greater than 0.");
            RuleFor(x => x.roomId).NotEmpty().WithMessage("Room ID is required.")
                .GreaterThan(0).WithMessage("Room ID must be greater than 0.");
        }
    }
}
using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.Rooms;

public record AddRoomViewModel(int ApartmentId, string Description, int NumberOfBeds, int Price, string ImageUrl);

public class AddRoomViewModelValidation : AbstractValidator<AddRoomViewModel>
{
    public AddRoomViewModelValidation()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.NumberOfBeds)
            .NotEmpty().WithMessage("Number of beds is required.")
            .GreaterThan(0).WithMessage("Number of beds must be greater than 0.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
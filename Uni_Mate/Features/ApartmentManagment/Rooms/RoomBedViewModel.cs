using FluentValidation;

namespace ApartmentManagment.Features.ApartmentManagment.Rooms;

public record BedViewModel(bool IsAvailable, double Price);
public record RoomBedViewModel(string Description, int Price, string Image, List<BedViewModel> Beds);
public class RoomBedViewModelValidator : AbstractValidator<List<RoomBedViewModel>>
{
    public RoomBedViewModelValidator()
    {
        RuleForEach(x => x)
            .SetValidator(new RoomBedViewModeleValidator());
    }
}

public class RoomBedViewModeleValidator : AbstractValidator<RoomBedViewModel>
{
    public RoomBedViewModeleValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Image is required.");
        RuleFor(x => x.Beds)
            .NotEmpty().WithMessage("At least one bed is required.");
    }
}
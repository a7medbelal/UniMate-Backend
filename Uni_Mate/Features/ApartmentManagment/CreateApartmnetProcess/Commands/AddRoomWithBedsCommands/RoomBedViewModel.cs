using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.AddRoomWithBedsCommands;

public record BedViewModel(bool IsAvailable, decimal Price);
public class RoomBedViewModel
{
  public  string Description { get; set; }
  public decimal Price { get; set; }
  public bool HasAC {get ; set; }
  public int BedsNumber { get; set; }
  public  IFormFile Image { get; set; }
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
        RuleFor(x => x.BedsNumber)
            .NotEmpty().WithMessage("At least one bed is required.");
    }
}
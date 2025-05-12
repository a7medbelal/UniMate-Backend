using FluentValidation;

namespace ApartmentManagment.Features.ApartmentManagment.Rooms;

public record BedViewModel(bool IsAvailable, double Price);
public record RoomBedViewModel(string Description, int Price, string Image, List<BedViewModel> Beds);
public class RoomBedViewModelValidator : AbstractValidator<List<RoomBedViewModel>>
{
    public RoomBedViewModelValidator()
    {
  //      RuleForEach(x => x)
//            .SetValidator(new RoomBedViewModelValidator());
    }
}

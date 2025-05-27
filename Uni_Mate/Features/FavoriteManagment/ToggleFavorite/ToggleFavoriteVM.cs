using FluentValidation;

namespace Uni_Mate.Features.FavoriteManagment.ToggleFavorite
{

    public record ToggleFavoriteVM(int id);
    public class ToggleFavoriteVMValidator : AbstractValidator<ToggleFavoriteVM>
    {
        public ToggleFavoriteVMValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Apartment ID Cannot Be Empty")
                .GreaterThan(0)
                .WithMessage("Apartment ID Must Be Greater Than 0");
        }
    }
}

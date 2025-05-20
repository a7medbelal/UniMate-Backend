using FluentValidation;

namespace Uni_Mate.Features.FavoriteManagment.DelFavoriteApartment
{
    public record DelFavoriteApartVM(int id);
    public class DelFavoritedApartValidator : AbstractValidator<DelFavoriteApartVM>
    {
        public DelFavoritedApartValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Apartment ID Cannot Be Empty")
                .GreaterThan(0)
                .WithMessage("Apartment ID Must Be Greater Than 0");
        }
    }
}

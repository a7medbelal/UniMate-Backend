using FluentValidation;

namespace Uni_Mate.Features.FavoriteManagment.AddFavoriteApartment
{
    public record AddFavoriteApartVM(int id)
    {
    }

    public class AddFavoriteApartValidator : AbstractValidator<AddFavoriteApartVM>
    {
        public AddFavoriteApartValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("Apartment ID Cannot Be Empty")
                .GreaterThan(0)
                .WithMessage("Apartment ID Must Be Greater Than 0");
        }
    }
}

using FluentValidation;
namespace Uni_Mate.Features.OwnerManager.ToggleActive
{
    public record ToggleActiveVM(string? IdUser);

    public class ToggleActiveValidator : AbstractValidator<ToggleActiveVM>
    {
        public ToggleActiveValidator() {
            RuleFor(x => x.IdUser).NotEmpty()
            .WithMessage(x => x.IdUser)
            .Must(x => x.Length >= 13)
            .WithMessage("You Must Make The Length Is Less Than 13.");
        }
    }
    
}

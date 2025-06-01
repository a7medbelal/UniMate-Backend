using FluentValidation;

namespace Uni_Mate.Features.ExtraEndpoints.GetOwnerById
{
    public record GetOwnerByIDVM(string? OwnerId);

    public class Validator : AbstractValidator<GetOwnerByIDVM>
    {
        public Validator()
        {
            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Owner ID cannot be empty.");
        }
    }
}

using FluentValidation;

namespace Uni_Mate.Features.CommentManagement.GetComments
{
    public record GetCommentsViewMode(int ApartmentId);

    public class GetCommentsValidator: AbstractValidator<GetCommentsViewMode>
    {
        public GetCommentsValidator()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("Apartment ID cannot be empty.")
                .GreaterThan(0).WithMessage("Apartment ID must be greater than 0.");

        }
    }
}
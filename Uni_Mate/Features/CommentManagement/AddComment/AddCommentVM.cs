using FluentValidation;

namespace Uni_Mate.Features.CommentManagement.AddComment
{
    public record AddCommentVM(string? Message, int ApartmentId);

    public class AddCommentVMValidator : AbstractValidator<AddCommentVM>
    {
        public AddCommentVMValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message cannot be empty.")
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters.");
            RuleFor(x => x.ApartmentId)
                .GreaterThan(0).WithMessage("Apartment ID must be greater than 0.");
        }
    }
}

using FluentValidation;

namespace Uni_Mate.Features.CommentManagement.DeleteComment
{
    public record DeleteCommentVM(int CommentId, int ApartmentId);
    public class DeleteCommentVMValidator : AbstractValidator<DeleteCommentVM>
    {
        public DeleteCommentVMValidator()
        {
            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("Comment ID must be greater than 0.");
            RuleFor(x => x.ApartmentId)
                .GreaterThan(0).WithMessage("Apartment ID must be greater than 0.");
        }
    }
}

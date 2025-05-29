using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.DeleteCategory
{
    public record DeleteCategoryVM(int Id);

    public class DeleteCategoryVMValidator :AbstractValidator<DeleteCategoryVM>
    {
        public DeleteCategoryVMValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category ID cannot be empty.")
                .GreaterThan(0).WithMessage("Category ID must be greater than zero.");
        }
    }
}

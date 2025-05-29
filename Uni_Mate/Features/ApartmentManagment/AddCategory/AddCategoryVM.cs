using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.AddCategory
{
    public record AddCategoryVM(string? Name);

    public class AddCategoryVMValidator : AbstractValidator<AddCategoryVM>
    {
        public AddCategoryVMValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name cannot be empty.")
                .MaximumLength(30).WithMessage("Category name cannot exceed 100 characters.");
        }
    }   
}

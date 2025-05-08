using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.Facilites.CategoryWithFacilty
{
    public class CategoryFacilityViewModel
    {
        public string CategoryName { get; set; }
        public List<FacilityApartmentViewModel> facilities{ get; set; }
    }
    public class CategoryFacilityViewModelValidator : AbstractValidator<CategoryFacilityViewModel>
    {
        public CategoryFacilityViewModelValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(x => x.facilities)
                .NotNull().WithMessage("Facilities list is required.")
                .ForEach(f => f.SetValidator(new FacilityApartmentViewModelValidator()));
        }
    }


    public class FacilityApartmentViewModel
    {
        public bool IsSelected { get; set; }
        public int FacilityId { get; set; }
    }
    public class FacilityApartmentViewModelValidator : AbstractValidator<FacilityApartmentViewModel>
    {
        public FacilityApartmentViewModelValidator()
        {
            RuleFor(x => x.FacilityId)
                .GreaterThan(0).WithMessage("FacilityId must be a positive number.");
        }
    }


    public class CategoryFacilityListValidator : AbstractValidator<List<CategoryFacilityViewModel>>
    {
        public CategoryFacilityListValidator()
        {
            RuleForEach(x => x).SetValidator(new CategoryFacilityViewModelValidator());

            RuleFor(x => x)
                .Must(list => list.Any(cat => cat.facilities.Any(f => f.IsSelected)))
                .WithMessage("At least one facility must be selected across all categories.");
        }
    }

}

using FluentValidation;

namespace Uni_Mate.Features.ApartmentManagment.CreateApartmnetProcess.Commands.CategoryWithFaciltyCommand
{
    public class CategoryFacilityViewModel
    {
        public string Name { get; set; }
        public List<FacilityApartmentViewModel> Facilities { get; set; }
    }
    public class CategoryFacilityViewModelValidator : AbstractValidator<CategoryFacilityViewModel>
    {
        public CategoryFacilityViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
               .Matches(@"^[\p{L}\d\s.,\-_]+$").WithMessage("FAcility must contain letters, digits, spaces, and allowed punctuation only.");

            RuleFor(x => x.Facilities)
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


    public class LFacilityApartmentViewModelValidator : AbstractValidator<List<FacilityApartmentViewModel>>
    {
        public LFacilityApartmentViewModelValidator()
        {
            RuleForEach(x => x).SetValidator(new FacilityApartmentViewModelValidator());
        }
    }

    //public class CategoryFacilityListValidator : AbstractValidator<List<CategoryFacilityViewModel>>
    //{
    //    public CategoryFacilityListValidator()
    //    {
    //        RuleForEach(x => x).SetValidator(new CategoryFacilityViewModelValidator());

    //        RuleFor(x => x)
    //            .Must(list => list.Any(cat => cat.Facilities.Any(f => f.IsSelected)))
    //            .WithMessage("At least one facility must be selected across all categories.");
    //    }
    //}

}

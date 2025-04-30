using FluentValidation;

namespace Uni_Mate.Features.StudentManager.UpdateProfileSave
{
    public record UpdateProfileSaveVM(string? FirstName, string? LastName, string? Governorate, string? Address, string? BriefOverView);
    

    public class UpdateProfileSaveVMValidation : AbstractValidator<UpdateProfileSaveVM>
    {
        public UpdateProfileSaveVMValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 20).WithMessage("First name must be between 2 and 50 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 20).WithMessage("Last name must be between 2 and 50 characters.");
            RuleFor(x => x.Governorate)
                .Length(4, 20).WithMessage("Governorate must be between 2 and 50 characters.");
            RuleFor(x => x.Address)
                .Length(5, 100).WithMessage("Address must be between 5 and 100 characters.");
            RuleFor(x => x.BriefOverView)
                .Length(10, 200).WithMessage("Brief overview must be between 10 and 200 characters.");
        }
    }
}

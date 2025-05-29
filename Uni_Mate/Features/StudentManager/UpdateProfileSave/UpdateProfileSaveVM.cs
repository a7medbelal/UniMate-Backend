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
	            .Length(2, 20).WithMessage("First name must be between 2 and 20 characters.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("First name must contain letters only.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Last name is required.")
				.Length(2, 20).WithMessage("Last name must be between 2 and 20 characters.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("Last name must contain letters only.");

			RuleFor(x => x.Governorate)
                .Length(2, 20).WithMessage("Governorate must be between 2 and 50 characters.")
				.Matches(@"^[\p{L}\s\u0621-\u064A]+$").WithMessage("Governorate must contain letters only.");

			RuleFor(x => x.Address)
                .Length(5, 100).WithMessage("Address must be between 5 and 100 characters.")
				.Matches(@"^[\p{L}\u0621-\u064A\d .,\-_\\r\\n]+$").WithMessage("Address must contain letters, numbers, spaces, and allowed punctuation (. , - _).");

			RuleFor(x => x.BriefOverView)
                .Length(2, 200).WithMessage("Brief overview must be between 10 and 200 characters.")
				.Matches(@"^[\p{L}\u0621-\u064A\d .,\-_\\r\\n]+$").WithMessage("Brief overview must contain letters, numbers, spaces, and punctuation only.");
		}
    }
}